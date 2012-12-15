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

using System.Data;

namespace OpenWebby.Data.Common
{
    /// <summary>
    /// Represents a connection to a database. This class cannot be inherited.
    /// </summary>
    public sealed class Connection : IDbConnection
    {
        public Connection(IDbConnection connection)
        {
            CurrentConnection = connection;
        }

        private IDbConnection CurrentConnection { get; set; }

        public IDbTransaction BeginTransaction(IsolationLevel il)
        {
            return CurrentConnection.BeginTransaction(il);
        }

        public IDbTransaction BeginTransaction()
        {
            return CurrentConnection.BeginTransaction();
        }

        public void ChangeDatabase(string databaseName)
        {
            CurrentConnection.ChangeDatabase(databaseName);
        }

        public void Close()
        {
            if (CurrentConnection.State != ConnectionState.Closed)
            {
                CurrentConnection.Close();
            }
        }

        public string ConnectionString
        {
            get
            {
                return CurrentConnection.ConnectionString;
            }
            set
            {
                CurrentConnection.ConnectionString = value;
            }
        }

        public int ConnectionTimeout
        {
            get { return CurrentConnection.ConnectionTimeout; }
        }

        public IDbCommand CreateCommand()
        {
            return CurrentConnection.CreateCommand();
        }

        public string Database
        {
            get { return CurrentConnection.Database; }
        }

        public void Open()
        {
            if (CurrentConnection.State != ConnectionState.Open)
            {
                CurrentConnection.Open();
            }
        }

        public ConnectionState State
        {
            get { return CurrentConnection.State; }
        }

        public void Dispose()
        {
            CurrentConnection.Dispose();
        }
    }
}
