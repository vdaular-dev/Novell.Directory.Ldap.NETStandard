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

namespace Novell.Directory.Ldap.Asn1
{
    /// <summary>
    ///     This abstract class is the base class
    ///     for all Asn1 numeric (integral) types. These include
    ///     Asn1Integer and Asn1Enumerated.
    /// </summary>
    public abstract class Asn1Numeric : Asn1Object
    {
        private readonly long _content;

        internal Asn1Numeric(Asn1Identifier id, int content)
            : base(id)
        {
            _content = content;
        }

        internal Asn1Numeric(Asn1Identifier id, long content)
            : base(id)
        {
            _content = content;
        }

        /// <summary> Returns the content of this Asn1Numeric object as an int.</summary>
        public int IntValue()
        {
            return (int)_content;
        }

        /// <summary> Returns the content of this Asn1Numeric object as a long.</summary>
        public long LongValue()
        {
            return _content;
        }
    }
}
