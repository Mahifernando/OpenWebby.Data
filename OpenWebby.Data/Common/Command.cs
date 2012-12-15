// ------------------------------------------------------------------------------
//                                                                              |
//   Copyright 2012 OpenWebby.                                                  |
//   Author : Mahi Fernando.                                                    |
//                                                                              |
//   Licensed under the Apache License, Version 2.0 (the "License");            |
//   you may not use this file except in compliance with the License.           |
//   You may obtain a copy of the License at                                    |
//                                                                              |
//       http://www.apache.org/licenses/LICENSE-2.0                             |
//                                                                              |
//   Unless required by applicable law or agreed to in writing, software        |
//   distributed under the License is distributed on an "AS IS" BASIS,          |
//   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.   |
//   See the License for the specific language governing permissions and        |
//   limitations under the License.                                             |
//                                                                              |
// ------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Diagnostics;
using OpenWebby.Data.Attributes;

namespace OpenWebby.Data.Common
{
    /// <summary>
    /// Represents an SQL statement that is executed while connected to a data source. 
    /// This class cannot be inherited.
    /// </summary>
    public sealed class Command : ICommand
    {
        private StackTrace _stackTrace;
        private IDbConnection _connection;
        private Transaction _transaction;
        
        public IDbConnection Connection {
            get { return _connection; }
        }

        public Transaction Transaction {
            get { 
                return _transaction; 
            }
        }        
        
        public Command(IDbConnection connection, StackTrace stackTrace)
        {
            if (null == connection) { throw new ArgumentNullException("connection"); }
            if (null == stackTrace) { throw new ArgumentNullException("stackTrace"); }

            _connection = connection;
            _stackTrace = stackTrace;            
        }

        public int ExecuteNonQuery(string commandText, ParameterFactory parameterCollection, CommandType commandType)
        {
            return Execute(c => c.ExecuteNonQuery(), commandText, parameterCollection, commandType);

        }

        public object ExecuteScalar(string commandText, ParameterFactory parameterCollection, CommandType commandType)
        {
            return Execute(c => c.ExecuteScalar(), commandText, parameterCollection, commandType);
        }

        public DataReader ExecuteReader(string commandText, ParameterFactory parameterCollection, CommandType commandType)
        {
            return new DataReader(Execute(c => (DbDataReader)c.ExecuteReader(), commandText, parameterCollection, commandType), _transaction);            
        }

        public DataTable ExecuteDataTable(DbDataAdapter adapter, string commandText, ParameterFactory parameterCollection, CommandType commandType)
        {
            DataTable datatable = new DataTable();
            int output = ExecuteAdapter(a => a.Fill(datatable), adapter, commandText, parameterCollection, commandType);

            return datatable;
        }

        public DataSet ExecuteDataSet(DbDataAdapter adapter, string commandText, ParameterFactory parameterCollection, CommandType commandType)
        {
            DataSet dataset = new DataSet();
            int output = ExecuteAdapter(a => a.Fill(dataset), adapter, commandText, parameterCollection, commandType);

            return dataset;
        }

        private IDbCommand BuildCommand(string commandText, ParameterFactory parameterCollection, CommandType commandType)
        {
            IDbCommand outCommand = Connection.CreateCommand();
            outCommand.CommandText = commandText;
            outCommand.CommandType = commandType; 

            if (null != parameterCollection)
            {
                foreach (DbParameter parameter in parameterCollection.Parameters)
                {
                    outCommand.Parameters.Add(parameter);
                }
            }

            return outCommand;
        }

        private T Execute<T>(Func<IDbCommand, T> dbCommand, string commandText, ParameterFactory parameterCollection, CommandType commandType)
        {
            T output = default(T);

            IDbCommand command = BuildCommand(commandText, parameterCollection, commandType);

            TransactionSupportAttribute[] TransactionAttributes = Trace.GetCustomAttribute(_stackTrace);

            if (Trace.IsTrasactionSupport(TransactionAttributes))
            {
                try
                {
                    BeginTransaction(TransactionAttributes, command);
                    output = dbCommand(command);
                    if (!IsDbDataReaderOpen(output))
                    {
                        _transaction.Commit();
                    }
                }
                catch (Exception ex)
                {
                    OnError(ex);
                }
            }
            else
            {
                try
                {
                    Connection.Open();
                    output = dbCommand(command);
                    if (!IsDbDataReaderOpen(output))
                    {
                        Connection.Close();
                    }                                        
                }
                catch (Exception ex)
                {
                    OnError(ex);
                }
            }

            return output;
        }

        private T ExecuteAdapter<T>(Func<DbDataAdapter, T> dbCommand, DbDataAdapter adapter, string commandText, ParameterFactory parameterCollection, CommandType commandType) 
        {
            T output = default(T);

            IDbCommand command = BuildCommand(commandText, parameterCollection, commandType);
            adapter.SelectCommand = (DbCommand)command;

            TransactionSupportAttribute[] TransactionAttributes = Trace.GetCustomAttribute(_stackTrace);

            if (Trace.IsTrasactionSupport(TransactionAttributes))
            {
                try
                {
                    BeginTransaction(TransactionAttributes, command);
                    output = dbCommand(adapter);
                    _transaction.Commit();                    
                }
                catch (Exception ex)
                {
                    OnError(ex);
                }
            }
            else
            {
                try
                {
                    Connection.Open();
                    output = dbCommand(adapter);
                    Connection.Close();
                }
                catch (Exception ex)
                {
                    OnError(ex);
                }
            }

            return output;
        }

        private void BeginTransaction(TransactionSupportAttribute[] TransactionAttributes, IDbCommand command)
        {
            TransactionSupportAttribute TranAttribute = TransactionAttributes[0];

            _transaction = null != TransactionPool.GetObject(TranAttribute.Key).Value ? TransactionPool.GetObject(TranAttribute.Key).Value : null;
            if (_transaction == null)
            {
                TransactionPool.Instances.Add(new KeyValuePair<string, Transaction>(TranAttribute.Key, new Transaction((Connection)Connection)));
                _transaction = TransactionPool.GetObject(TranAttribute.Key).Value;
                _transaction.TransactionAttributes = TransactionAttributes;

                if (_transaction.TransactionAttributes[0].Tasks > 0 && _transaction.RunningStep == 0)
                {
                    command.Transaction = _transaction.Begin(_transaction.TransactionAttributes[0].Isolation);
                }
            }
            else
            {
                command.Transaction = _transaction.CurrentTransaction;
            }
        }

        private void OnError(Exception ex)
        {
            if (null != _transaction)
            {
                if (_transaction.IsRuning) { _transaction.Rollback(); }
                _transaction = null;
            }
            throw ex;
        }

        private bool IsDbDataReaderOpen(object dataReader)
        {
            return (null != dataReader && dataReader.GetType().BaseType == typeof(DbDataReader));
        }
    }
}
