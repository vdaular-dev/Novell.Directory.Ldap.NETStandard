﻿/******************************************************************************
* The MIT License
* Copyright (c) 2003 Novell Inc.  www.novell.com
*
* Permission is hereby granted, free of charge, to any person obtaining  a copy
* of this software and associated documentation files (the Software), to deal
* in the Software without restriction, including  without limitation the rights
* to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
* copies of the Software, and to  permit persons to whom the Software is
* furnished to do so, subject to the following conditions:
*
* The above copyright notice and this permission notice shall be included in
* all copies or substantial portions of the Software.
*
* THE SOFTWARE IS PROVIDED AS IS, WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
* IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
* FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
* AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
* LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
* OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
* SOFTWARE.
*******************************************************************************/

using System.Threading;

namespace Novell.Directory.Ldap
{
    /// <summary>
    ///     A mechanism for queuing asynchronous search results
    ///     received from a server.
    /// </summary>
    /// <seealso cref="LdapConnection.SearchAsync(string,int,string,string[],bool,LdapSearchQueue,CancellationToken)"/>
    /// <seealso cref="LdapConnection.SearchAsync(string,int,string,string[],bool,LdapSearchQueue,LdapSearchConstraints,CancellationToken)"/>
    /// <seealso cref="LdapResponseQueue"/>
    public class LdapSearchQueue : LdapMessageQueue
    {
        /// <summary>
        ///     Constructs a response queue using a specific client queue.
        /// </summary>
        /// <param name="agent">
        ///     The message agent to associate with this queue.
        /// </param>
        internal LdapSearchQueue(MessageAgent agent)
            : base("LdapSearchQueue", agent)
        {
        }
    }
}
