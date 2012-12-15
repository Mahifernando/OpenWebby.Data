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
using System.Data.Common;

namespace OpenWebby.Data.Common
{
    /// <summary>
    /// Represents an SQL statement that is executed while connected to a data source.
    /// It is implemented by OpenWebby data access classes.
    /// </summary>
    public interface ICommand
    {
        IDbConnection Connection { get; }
        Transaction Transaction { get; }

        int ExecuteNonQuery(string commandText, ParameterFactory parameterCollection, CommandType commandType);
        object ExecuteScalar(string commandText, ParameterFactory parameterCollection, CommandType commandType);
        DataReader ExecuteReader(string commandText, ParameterFactory parameterCollection, CommandType commandType);
        DataTable ExecuteDataTable(DbDataAdapter adapter, string commandText, ParameterFactory parameterCollection, CommandType commandType);
        DataSet ExecuteDataSet(DbDataAdapter adapter, string commandText, ParameterFactory parameterCollection, CommandType commandType);
    }
}
