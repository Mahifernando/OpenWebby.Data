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
using System.Runtime.InteropServices;

namespace OpenWebby.Data.Attributes
{
    /// <summary>
    /// Specify the implementation of transaction support within an invoker method. This class cannot be inherited.
    /// </summary>
    [ComVisible(true)]
    [AttributeUsage(AttributeTargets.Method)]
    public sealed class TransactionSupportAttribute: Attribute
    {
        private bool _inTransaction = true;
        private string _description;
        private string _key;
        private int _tasks = 0;
        private IsolationLevel _isolation = IsolationLevel.Unspecified;

        /// <summary>
        /// Initialize new instance of the TransactionSupportAttribute class.
        /// </summary>
        /// <param name="key">Guid key to identify the invoker method in runtime.</param>
        /// <param name="tasks">Number of tasks involve in this trasaction.</param>
        public TransactionSupportAttribute(string key, int tasks) 
        {
            this._key = key;
            this._tasks = tasks;
        }

        /// <summary>
        /// [Optional] Default value is true. Set false to by pass the transaction implementation.
        /// </summary>
        public bool InTransaction { get { return this._inTransaction; } set { this._inTransaction = value; } }

        /// <summary>
        /// [Optional] Description of the underlying transaction tasks.
        /// </summary>
        public string Description { get { return this._description; } set { this._description = value; } }

        /// <summary>
        /// Get guid key to identify the invoker method in runtime.
        /// </summary>
        public string Key { get { return this._key; } }

        /// <summary>
        /// Get number of tasks involve in this trasaction.
        /// </summary>
        public int Tasks { get { return this._tasks; } }

        /// <summary>
        /// Get/Set the transaction locking behavior for the connection.
        /// Default IsolationLevel is Unspecified, 
        /// this means the transaction executes according to the isolation level that is determined by the driver that is being used.
        /// </summary>
        public IsolationLevel Isolation { get { return _isolation; } set { _isolation = value; } }                
    }
}
