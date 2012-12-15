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

using OpenWebby.Data.Attributes;

namespace OpenWebby.Data.Common
{
    /// <summary>
    /// Provide helper methods to trace invoker attributes.
    /// </summary>
    public static class Trace
    {
        /// <summary>
        /// Populates TransactionSupportAttribute array within the last frame of given stacktrace instance.
        /// </summary>
        /// <param name="stack">Instance of current stack trace in the type of System.Diagnostics.StackTrace</param>
        /// <returns></returns>
        public static TransactionSupportAttribute[] GetCustomAttribute(System.Diagnostics.StackTrace stack)
        {
            if (null == stack) { return null; }
            return (TransactionSupportAttribute[])stack.GetFrame(1).GetMethod().GetCustomAttributes(typeof(TransactionSupportAttribute), true);
        }

        /// <summary>
        /// Specify given TransactionSupportAttribute array has attribute values or not.
        /// </summary>
        /// <param name="attributes">A array of type OpenWebby.Data.Attributes.TransactionSupportAttribute</param>
        /// <returns>true/false</returns>
        public static bool HasAttribute(TransactionSupportAttribute[] attributes)
        {
            return null != attributes;
        }

        /// <summary>
        /// Specify whether transaction is suppoted by current invoker.
        /// </summary>
        /// <param name="transactionSupportAttributes">A array of type OpenWebby.Data.Attributes.TransactionSupportAttribute</param>
        /// <returns>true/false</returns>
        public static bool IsTrasactionSupport(TransactionSupportAttribute[] transactionSupportAttributes)
        {
            if (HasAttribute(transactionSupportAttributes))
            {
                return transactionSupportAttributes.Length > 0 ? transactionSupportAttributes[0].InTransaction : false;
            }
            return false;
        }
    }
}
