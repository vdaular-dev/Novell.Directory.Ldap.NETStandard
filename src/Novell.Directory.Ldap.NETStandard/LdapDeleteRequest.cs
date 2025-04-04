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

using Novell.Directory.Ldap.Rfc2251;
using System.Threading;

namespace Novell.Directory.Ldap
{
    /// <summary>
    ///     Represents a request to delete an entry.
    /// </summary>
    /// <seealso cref="LdapConnection.SendRequestAsync(LdapMessage,LdapMessageQueue,CancellationToken)"/>
    /// <seealso cref="LdapConnection.SendRequestAsync(LdapMessage,LdapMessageQueue,LdapConstraints,CancellationToken)"/>
    /*
     *       DelRequest ::= [APPLICATION 10] LdapDN
     */
    public class LdapDeleteRequest : LdapMessage
    {
        public override DebugId DebugId { get; } = DebugId.ForType<LdapDeleteRequest>();

        /// <summary>
        ///     Constructs a request to delete an entry from the directory.
        /// </summary>
        /// <param name="dn">
        ///     the dn of the entry to delete.
        /// </param>
        /// <param name="cont">
        ///     Any controls that apply to the abandon request
        ///     or null if none.
        /// </param>
        public LdapDeleteRequest(string dn, LdapControl[] cont)
            : base(DelRequest, new RfcDelRequest(dn), cont)
        {
        }

        /// <summary>
        ///     Returns of the dn of the entry to delete from the directory.
        /// </summary>
        /// <returns>
        ///     the dn of the entry to delete.
        /// </returns>
        public string Dn => Asn1Object.RequestDn;

        /// <summary>
        ///     Return an Asn1 representation of this delete request
        ///     #return an Asn1 representation of this object.
        /// </summary>
        public override string ToString()
        {
            return Asn1Object.ToString();
        }
    }
}
