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

namespace OpenWebby.Data.Common
{
    /// <summary>
    /// Implement object pool for transactions
    /// </summary>
    public sealed class TransactionPool
    {
        /// <summary>
        /// Maintain transaction object collection.
        /// </summary>
        public static List<KeyValuePair<string, Transaction>> Instances = new List<KeyValuePair<string, Transaction>>();

        /// <summary>
        /// Get transaction object from the pooled collection.
        /// </summary>
        /// <param name="key">transaction key</param>
        /// <returns>A Key/Value pair, the Key of type OpenWebby.Data.Common.Transaction</returns>
        public static KeyValuePair<string, Transaction> GetObject(string key)
        {
            return Instances.Find(x => x.Key == key);
        }

        /// <summary>
        /// Remove transaction from the pooled object collection.
        /// </summary>
        /// <param name="key">transaction key</param>
        public static void Remove(string key)
        {
            Instances.Remove(GetObject(key));
        }        
    }
}


