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
using System.Data;
using System.Data.Common;
using System.Diagnostics;


namespace OpenWebby.Data.Common
{
    /// <summary>
    /// Base class implements a set of methods to access data source.
    /// </summary>
    public abstract class DataAccess
    {
        private Connection _connection;
        private Command _command;
        private DbProviderFactory _dbprovider;

        public Connection Connection { get { return _connection; } }

        public DataAccess(string connectionString, string provider)
        {
            _dbprovider = DbProviderFactories.GetFactory(provider);
            DbConnection dbconnection = _dbprovider.CreateConnection();
            dbconnection.ConnectionString = connectionString;          

            _connection = new Connection(dbconnection);            
        }

        public int ExecuteNonQuery(string commandText, ParameterFactory parameterCollection, CommandType commandType = CommandType.Text)
        {
            return Execute(c => c.ExecuteNonQuery(commandText, parameterCollection, commandType), new StackTrace());
        }

        public object ExecuteScalar(string commandText, CommandType commandType = CommandType.Text)
        {
            return Execute(c => c.ExecuteScalar(commandText, null, commandType), new StackTrace());
        }

        public object ExecuteScalar(string commandText, ParameterFactory parameterCollection, CommandType commandType = CommandType.Text)
        {
            return Execute(c => c.ExecuteScalar(commandText, parameterCollection, commandType), new StackTrace());
        }

        public DataReader ExecuteReader(string commandText, ParameterFactory parameterCollection, CommandType commandType = CommandType.Text)
        {
            return Execute(c => c.ExecuteReader(commandText, parameterCollection, commandType), new StackTrace());
        }

        public DataTable ExecuteDataTable(string commandText, ParameterFactory parameterCollection, CommandType commandType = CommandType.Text)
        {
            DbDataAdapter adapter = _dbprovider.CreateDataAdapter();
            return Execute(c => c.ExecuteDataTable(adapter, commandText, parameterCollection, commandType), new StackTrace());            
        }

        public DataSet ExecuteDataSet(string commandText, ParameterFactory parameterCollection, CommandType commandType = CommandType.Text)
        {
            DbDataAdapter adapter = _dbprovider.CreateDataAdapter();
            return Execute(c => c.ExecuteDataSet(adapter, commandText, parameterCollection, commandType), new StackTrace());
        }

        private T Execute<T>(Func<Command, T> databaseCommand, StackTrace stackTrace)
        {
            _command = new Command(_connection, stackTrace);
            return databaseCommand(_command);
        }
    }
}
