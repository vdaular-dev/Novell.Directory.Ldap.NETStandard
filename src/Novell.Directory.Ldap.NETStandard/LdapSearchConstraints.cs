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

using System.Collections;

namespace Novell.Directory.Ldap
{
    /// <summary>
    ///     Defines the options controlling search operations.
    ///     An LdapSearchConstraints object is always associated with an
    ///     LdapConnection object; its values can be changed with the
    ///     LdapConnection.setConstraints method, or overridden by passing
    ///     an LdapSearchConstraints object to the search operation.
    /// </summary>
    /// <seealso cref="LdapConstraints">
    /// </seealso>
    /// <seealso cref="LdapConnection.Constraints">
    /// </seealso>
    public class LdapSearchConstraints : LdapConstraints
    {
        /// <summary>
        ///     Indicates that aliases are never dereferenced.
        ///     DEREF_NEVER = 0.
        /// </summary>
        /// <seealso cref="Dereference">
        /// </seealso>
        /// <seealso cref="Dereference">
        /// </seealso>
        public const int DerefNever = 0;

        /// <summary>
        ///     Indicates that aliases are are derefrenced when
        ///     searching the entries beneath the starting point of the search,
        ///     but not when finding the starting entry.
        ///     DEREF_SEARCHING = 1.
        /// </summary>
        /// <seealso cref="Dereference">
        /// </seealso>
        /// <seealso cref="Dereference">
        /// </seealso>
        public const int DerefSearching = 1;

        /// <summary>
        ///     Indicates that aliases are dereferenced when
        ///     finding the starting point for the search,
        ///     but not when searching under that starting entry.
        ///     DEREF_FINDING = 2.
        /// </summary>
        /// <seealso cref="Dereference">
        /// </seealso>
        /// <seealso cref="Dereference">
        /// </seealso>
        public const int DerefFinding = 2;

        /// <summary>
        ///     Indicates that aliases are always dereferenced, both when
        ///     finding the starting point for the search, and also when
        ///     searching the entries beneath the starting entry.
        ///     DEREF_ALWAYS = 3.
        /// </summary>
        /// <seealso cref="Dereference">
        /// </seealso>
        /// <seealso cref="Dereference">
        /// </seealso>
        public const int DerefAlways = 3;

        public override DebugId DebugId { get; } = DebugId.ForType<LdapSearchConstraints>();

        /// <summary>
        ///     Constructs an LdapSearchConstraints object with a default set
        ///     of search constraints.
        /// </summary>
        public LdapSearchConstraints()
        {
        }

        /// <summary>
        ///     Constructs an LdapSearchConstraints object initialized with values
        ///     from an existing constraints object (LdapConstraints
        ///     or LdapSearchConstraints).
        /// </summary>
        public LdapSearchConstraints(LdapConstraints cons)
            : base(cons.TimeLimit, cons.ReferralFollowing, cons.getReferralHandler(), cons.HopLimit)
        {
            var lsc = cons.GetControls();
            if (lsc != null)
            {
                var generatedVar = new LdapControl[lsc.Length];
                lsc.CopyTo(generatedVar, 0);
                SetControls(generatedVar);
            }

            var lp = cons.Properties;
            if (lp != null)
            {
                Properties = (Hashtable)lp.Clone();
            }

            if (cons is LdapSearchConstraints scons)
            {
                ServerTimeLimit = scons.ServerTimeLimit;
                Dereference = scons.Dereference;
                MaxResults = scons.MaxResults;
                BatchSize = scons.BatchSize;
            }
        }

        /// <summary>
        ///     Constructs a new LdapSearchConstraints object and allows the
        ///     specification operational constraints in that object.
        /// </summary>
        /// <param name="msLimit">
        ///     The maximum time in milliseconds to wait for results.
        ///     The default is 0, which means that there is no
        ///     maximum time limit. This limit is enforced for an
        ///     operation by the API, not by the server.
        ///     The operation will be abandoned and terminated by the
        ///     API with an LdapException.Ldap_TIMEOUT if the
        ///     operation exceeds the time limit.
        /// </param>
        /// <param name="serverTimeLimit">
        ///     The maximum time in seconds that the server
        ///     should spend returning search results. This is a
        ///     server-enforced limit.  The default of 0 means
        ///     no time limit.
        ///     The operation will be terminated by the server with an
        ///     LdapException.TIME_LIMIT_EXCEEDED if the search
        ///     operation exceeds the time limit.
        /// </param>
        /// <param name="dereference">
        ///     Specifies when aliases should be dereferenced.
        ///     Must be either DEREF_NEVER, DEREF_FINDING,
        ///     DEREF_SEARCHING, or DEREF_ALWAYS from this class.
        ///     Default: DEREF_NEVER.
        /// </param>
        /// <param name="maxResults">
        ///     The maximum number of search results to return
        ///     for a search request.
        ///     The search operation will be terminated by the server
        ///     with an LdapException.SIZE_LIMIT_EXCEEDED if the
        ///     number of results exceed the maximum.
        ///     Default: 1000.
        /// </param>
        /// <param name="doReferrals">
        ///     Determines whether to automatically follow
        ///     referrals or not. Specify true to follow
        ///     referrals automatically, and false to throw
        ///     an LdapException.REFERRAL if the server responds
        ///     with a referral.
        ///     It is ignored for asynchronous operations.
        ///     Default: false.
        /// </param>
        /// <param name="batchSize">
        ///     The number of results to return in a batch. Specifying
        ///     0 means to block until all results are received.
        ///     Specifying 1 means to return results one result at a
        ///     time.  Default: 1.
        /// </param>
        /// <param name="handler">
        ///     The custom authentication handler called when
        ///     LdapConnection needs to authenticate, typically on
        ///     following a referral.  A null may be specified to
        ///     indicate default authentication processing, i.e.
        ///     referrals are followed with anonymous authentication.
        ///     ThE object may be an implemention of either the
        ///     the LdapBindHandler or LdapAuthHandler interface.
        ///     It is ignored for asynchronous operations.
        /// </param>
        /// <param name="hopLimit">
        ///     The maximum number of referrals to follow in a
        ///     sequence during automatic referral following.
        ///     The default value is 10. A value of 0 means no limit.
        ///     It is ignored for asynchronous operations.
        ///     The operation will be abandoned and terminated by the
        ///     API with an LdapException.REFERRAL_LIMIT_EXCEEDED if the
        ///     number of referrals in a sequence exceeds the limit.
        /// </param>
        /// <seealso cref="LdapException.LdapTimeout">
        /// </seealso>
        /// <seealso cref="LdapException.Referral">
        /// </seealso>
        /// <seealso cref="LdapException.SizeLimitExceeded">
        /// </seealso>
        /// <seealso cref="LdapException.TimeLimitExceeded">
        /// </seealso>
        public LdapSearchConstraints(int msLimit, int serverTimeLimit, int dereference, int maxResults,
            bool doReferrals,
            int batchSize, ILdapReferralHandler handler, int hopLimit)
            : base(msLimit, doReferrals, handler, hopLimit)
        {
            ServerTimeLimit = serverTimeLimit;
            Dereference = dereference;
            MaxResults = maxResults;
            BatchSize = batchSize;
        }

        /// <summary>
        ///     Specifies the number of results to return in a batch.
        ///     Specifying 0 means to block until all results are received.
        ///     Specifying 1 means to return results one result at a time.  Default: 1
        ///     This should be 0 if intermediate results are not needed,
        ///     and 1 if results are to be processed as they come in.  The
        ///     default is 1.
        /// </summary>
        /// <param name="value">
        ///     The number of results to block on.
        /// </param>
        /// <returns>
        ///     The the number of results to block on.
        /// </returns>
        public int BatchSize { get; set; } = 1;

        /// <summary>
        ///     Specifies when aliases should be dereferenced.
        ///     Returns one of the following:.
        ///     <ul>
        ///         <li>DEREF_NEVER</li>
        ///         <li>DEREF_FINDING</li>
        ///         <li>DEREF_SEARCHING</li>
        ///         <li>DEREF_ALWAYS</li>
        ///     </ul>
        /// </summary>
        /// <param name="value">
        ///     Specifies how aliases are dereference and can be set
        ///     to one of the following:.
        ///     <ul>
        ///         <li>DEREF_NEVER - do not dereference aliases</li>
        ///         <li>
        ///             DEREF_FINDING - dereference aliases when finding
        ///             the base object to start the search
        ///         </li>
        ///         <li>
        ///             DEREF_SEARCHING - dereference aliases when
        ///             searching but not when finding the base
        ///             object to start the search
        ///         </li>
        ///         <li>
        ///             DEREF_ALWAYS - dereference aliases when finding
        ///             the base object and when searching
        ///         </li>
        ///     </ul>
        /// </param>
        /// <returns>
        ///     The setting for dereferencing aliases.
        /// </returns>
        public int Dereference { get; set; } = DerefNever;

        /// <summary>
        ///     Sets the maximum number of search results to be returned from a
        ///     search operation. The value 0 means no limit.  The default is 1000.
        ///     The search operation will be terminated with an
        ///     LdapException.SIZE_LIMIT_EXCEEDED if the number of results
        ///     exceed the maximum.
        /// </summary>
        /// <param name="value">
        ///     Maximum number of search results to return.
        /// </param>
        /// <returns>
        ///     The value for the maximum number of results to return.
        /// </returns>
        /// <seealso cref="LdapException.SizeLimitExceeded"/>
        public int MaxResults { get; set; } = 1000;

        /// <summary>
        ///     Sets the maximum number of seconds that the server is to wait when
        ///     returning search results.
        ///     The search operation will be terminated with an
        ///     LdapException.TIME_LIMIT_EXCEEDED if the operation exceeds the time
        ///     limit.
        ///     The parameter is only recognized on search operations.
        /// </summary>
        /// <param name="value">
        ///     The number of seconds to wait for search results.
        /// </param>
        /// <returns>
        ///     The maximum number of seconds the server waits for search'
        ///     results.
        /// </returns>
        /// <seealso cref="LdapException.TimeLimitExceeded"/>
        public int ServerTimeLimit { get; set; }
    }
}
