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
using OpenWebby.Data.Attributes;

namespace OpenWebby.Data.Common
{
    /// <summary>
    /// Represents database transaction. This class cannot be inherited
    /// </summary>
    public sealed class Transaction : IDisposable
    {
        private int _runningStep = 0;
        private bool _isRuning = false;
        private TransactionSupportAttribute[] _transactionAttributes;
        private Connection _connection;
        private IDbTransaction _currentTransaction;

        public int RunningStep { get { return _runningStep; } set { _runningStep = value; ;} }

        public bool IsRuning { get { return _isRuning; } set { _isRuning = value; } }

        public TransactionSupportAttribute[] TransactionAttributes
        {
            get
            {
                return _transactionAttributes;
            }
            set
            {
                _transactionAttributes = value;
            }
        }

        private Connection Connection { get { return _connection; } set { _connection = value; } }

        public IDbTransaction CurrentTransaction { get { return _currentTransaction; } set { _currentTransaction = value; } }

        public Transaction(Connection connection)
        {
            Connection = connection;
        }

        public IDbTransaction Begin(IsolationLevel isolationLevel)
        {
            IsRuning = true;
            if (null != Connection)
            {
                if (Connection.State != ConnectionState.Open)
                {
                    Connection.Open();
                }

                CurrentTransaction = Connection.BeginTransaction(isolationLevel);
            }
            return CurrentTransaction;
        }

        public void Commit()
        {
            RunningStep += 1;

            if (TransactionAttributes[0].Tasks > 0
                && RunningStep != 0
                && TransactionAttributes[0].Tasks == RunningStep
                && null != CurrentTransaction)
            {
                CurrentTransaction.Commit();               

                if (Connection.State != ConnectionState.Closed)
                {
                    Connection.Close();                    
                }
                Dispose();
            }

        }

        public void Rollback()
        {
            CurrentTransaction.Rollback();

            if (Connection.State != ConnectionState.Closed)
            {
                Connection.Close();                
            }
            Dispose();

        }

        public void Dispose()
        {
            TransactionPool.Remove(TransactionAttributes[0].Key);
        }
    }
}
