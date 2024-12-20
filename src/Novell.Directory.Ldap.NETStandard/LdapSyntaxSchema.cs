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

using Novell.Directory.Ldap.Utilclass;
using System;
using System.Collections;
using System.IO;
using System.Text;

namespace Novell.Directory.Ldap
{
    /// <summary>
    ///     Represents a syntax definition in the directory schema.
    ///     The LdapSyntaxSchema class represents the definition of a syntax.  It is
    ///     used to discover the known set of syntaxes in effect for the subschema.
    ///     Although this extends LdapSchemaElement, it does not use the name or
    ///     obsolete members. Therefore, calls to the getName method always return
    ///     null and to the isObsolete method always returns false. There is also no
    ///     matching getSyntaxNames method in LdapSchema. Note also that adding and
    ///     removing syntaxes is not typically a supported feature of Ldap servers.
    /// </summary>
    public class LdapSyntaxSchema : LdapSchemaElement
    {
        /// <summary>
        ///     Constructs a syntax for adding to or deleting from the schema.
        ///     Adding and removing syntaxes is not typically a supported
        ///     feature of Ldap servers. Novell eDirectory does not allow syntaxes to
        ///     be added or removed.
        /// </summary>
        /// <param name="oid">
        ///     The unique object identifier of the syntax - in
        ///     dotted numerical format.
        /// </param>
        /// <param name="description">
        ///     An optional description of the syntax.
        /// </param>
        public LdapSyntaxSchema(string oid, string description)
            : base(LdapSchema.SchemaTypeNames[LdapSchema.Syntax])
        {
            Id = oid;
            Description = description;
            Value = FormatString();
        }

        /// <summary>
        ///     Constructs a syntax from the raw string value returned on a schema
        ///     query for LdapSyntaxes.
        /// </summary>
        /// <param name="raw">
        ///     The raw string value returned from a schema
        ///     query for ldapSyntaxes.
        /// </param>
        public LdapSyntaxSchema(string raw)
            : base(LdapSchema.SchemaTypeNames[LdapSchema.Syntax])
        {
            try
            {
                var parser = new SchemaParser(raw);

                if (parser.Id != null)
                {
                    Id = parser.Id;
                }

                if (parser.Description != null)
                {
                    Description = parser.Description;
                }

                foreach (var attrQualifier in parser.Qualifiers)
                {
                    SetQualifier(attrQualifier.Name, attrQualifier.Values);
                }

                Value = FormatString();
            }
            catch (IOException e)
            {
                throw new Exception(e.ToString());
            }
        }

        /// <summary>
        ///     Returns a string in a format suitable for directly adding to a
        ///     directory, as a value of the particular schema element class.
        /// </summary>
        /// <returns>
        ///     A string representation of the syntax's definition.
        /// </returns>
        protected override string FormatString()
        {
            var valueBuffer = new StringBuilder("( ");
            string token;

            if ((token = Id) != null)
            {
                valueBuffer.Append(token);
            }

            if ((token = Description) != null)
            {
                valueBuffer.Append(" DESC ");
                valueBuffer.Append("'" + token + "'");
            }

            foreach (var qualName in QualifierNames)
            {
                valueBuffer.Append(" " + qualName + " ");
                var qualValue = GetQualifier(qualName);
                if (qualValue is { Length: > 1 })
                {
                    valueBuffer.Append("( '");

                    valueBuffer.Append(qualValue[0]);

                    for (var i = 1; i < qualValue.Length; i++)
                    {
                        valueBuffer.Append("' '");
                        valueBuffer.Append(qualValue[i]);
                    }

                    valueBuffer.Append("' )");
                }
            }

            valueBuffer.Append(" )");
            return valueBuffer.ToString();
        }
    }
}
