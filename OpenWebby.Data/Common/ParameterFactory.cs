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

using System.Collections.Generic;
using System.Data.Common;
using System.Data;

namespace OpenWebby.Data.Common
{
    /// <summary>
    ///  Represents a set of methods for creating instances of a provider's implementation
    ///  of the data parameters. This class cannot be inherited.
    /// </summary>
    public sealed class ParameterFactory
    {
        private List<DbParameter> _parameters;
        private DbParameter _dataParameter;
        private DataAccess _database;

        public List<DbParameter> Parameters { get { return _parameters; } }

        public ParameterFactory(DataAccess database)
        {
            _database = database;
            _parameters = new List<DbParameter>();
        }

        internal DbParameter CreateParameter()
        {
            return (DbParameter)_database.Connection.CreateCommand().CreateParameter();
        }

        internal void Add(DbParameter parameter)
        {
            _parameters.Add(parameter);
        }

        public void Add(string parameterName, object value)
        {
            _dataParameter = CreateParameter();

            _dataParameter.ParameterName = parameterName;
            _dataParameter.Value = value;

            Add(_dataParameter);
        }

        public void Add(string parameterName, object value, DbType dbType)
        {
            _dataParameter = CreateParameter();

            _dataParameter.ParameterName = parameterName;
            _dataParameter.Value = value;
            _dataParameter.DbType = dbType;

            Add(_dataParameter);
        }

        public void Add(string parameterName, object value, DbType dbType, bool isNullable)
        {
            _dataParameter = CreateParameter();

            _dataParameter.ParameterName = parameterName;
            _dataParameter.Value = value;            
            _dataParameter.DbType = dbType;
            _dataParameter.IsNullable = isNullable;

            Add(_dataParameter);
        }

        public void Add(string parameterName, object value, ParameterDirection direction)
        {
            _dataParameter = CreateParameter();
            
            _dataParameter.ParameterName = parameterName;
            _dataParameter.Value = value;
            _dataParameter.Direction = direction;

            Add(_dataParameter);
        }

        public void Add(string parameterName, object value, ParameterDirection direction, DbType dbType)
        {
            _dataParameter = CreateParameter();
            
            _dataParameter.ParameterName = parameterName;
            _dataParameter.Value = value;
            _dataParameter.Direction = direction;
            _dataParameter.DbType = dbType;

            Add(_dataParameter);
        }

        public void Add(string parameterName, object value, ParameterDirection direction, DbType dbType, bool isNullable)
        {
            _dataParameter = CreateParameter();

            _dataParameter.ParameterName = parameterName;
            _dataParameter.Value = value;
            _dataParameter.Direction = direction;
            _dataParameter.DbType = dbType;
            _dataParameter.IsNullable = isNullable;

            Add(_dataParameter);
        }

        public void Add(string parameterName, object value, ParameterDirection direction, DbType dbType, bool isNullable, int size)
        {
            _dataParameter = CreateParameter();

            _dataParameter.ParameterName = parameterName;
            _dataParameter.Value = value;
            _dataParameter.Direction = direction;
            _dataParameter.DbType = dbType;
            _dataParameter.IsNullable = isNullable;
            _dataParameter.Size = size;

            Add(_dataParameter);
        }

        public void Add(string parameterName, object value, ParameterDirection direction, DbType dbType, bool isNullable, int size, string sourceColumn)
        {
            _dataParameter = CreateParameter();

            _dataParameter.ParameterName = parameterName;
            _dataParameter.Value = value;
            _dataParameter.Direction = direction;
            _dataParameter.DbType = dbType;
            _dataParameter.IsNullable = isNullable;
            _dataParameter.Size = size;
            _dataParameter.SourceColumn = sourceColumn;

            Add(_dataParameter);
        }

        public void Add(string parameterName, object value, ParameterDirection direction, DbType dbType, bool isNullable, int size, string sourceColumn, bool sourceColumnNullMapping)
        {
            _dataParameter = CreateParameter();

            _dataParameter.ParameterName = parameterName;
            _dataParameter.Value = value;
            _dataParameter.Direction = direction;
            _dataParameter.DbType = dbType;
            _dataParameter.IsNullable = isNullable;
            _dataParameter.Size = size;
            _dataParameter.SourceColumn = sourceColumn;
            _dataParameter.SourceColumnNullMapping = sourceColumnNullMapping;

            Add(_dataParameter);
        }

        public void Add(string parameterName, object value, ParameterDirection direction, DbType dbType, bool isNullable, int size, string sourceColumn, bool sourceColumnNullMapping, DataRowVersion sourceVersion)
        {
            _dataParameter = CreateParameter();

            _dataParameter.ParameterName = parameterName;
            _dataParameter.Value = value;
            _dataParameter.Direction = direction;
            _dataParameter.DbType = dbType;
            _dataParameter.IsNullable = isNullable;
            _dataParameter.Size = size;
            _dataParameter.SourceColumn = sourceColumn;
            _dataParameter.SourceColumnNullMapping = sourceColumnNullMapping;
            _dataParameter.SourceVersion = sourceVersion;

            Add(_dataParameter);
        }
                
    }
}
