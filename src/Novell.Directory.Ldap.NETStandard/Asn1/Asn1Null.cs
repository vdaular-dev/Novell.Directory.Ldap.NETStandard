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

using System.IO;

namespace Novell.Directory.Ldap.Asn1
{
    /// <summary> This class represents the ASN.1 NULL type.</summary>
    public class Asn1Null : Asn1Object
    {
        /// <summary> ASN.1 NULL tag definition.</summary>
        public const int Tag = 0x05;

        /// <summary> ID is added for Optimization.</summary>
        /// <summary>
        ///     ID needs only be one Value for every instance,
        ///     thus we create it only once.
        /// </summary>
        private static readonly Asn1Identifier Id = new Asn1Identifier(Asn1Identifier.Universal, false, Tag);

        /* Constructor for Asn1Null
                */

        /// <summary>
        ///     Call this constructor to construct a new Asn1Null
        ///     object.
        /// </summary>
        public Asn1Null()
            : base(Id)
        {
        }

        /* Asn1Object implementation
        */

        /// <summary>
        ///     Call this method to encode the current instance into the
        ///     specified output stream using the specified encoder object.
        /// </summary>
        /// <param name="enc">
        ///     Encoder object to use when encoding self.
        /// </param>
        /// <param name="output">
        ///     The output stream onto which the encoded byte
        ///     stream is written.
        /// </param>
        public override void Encode(IAsn1Encoder enc, Stream output)
        {
            enc.Encode(this, output);
        }

        /* Asn1Null specific methods
        */

        /// <summary> Return a String representation of this Asn1Null object.</summary>
        public override string ToString()
        {
            return base.ToString() + "NULL: \"\"";
        }
    }
}
