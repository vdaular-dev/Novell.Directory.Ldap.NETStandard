﻿using Novell.Directory.Ldap.NETStandard.FunctionalTests.Helpers;
using Novell.Directory.Ldap.NETStandard.UnitTests.Helpers;
using Novell.Directory.Ldap.Sasl;
using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using Xunit;

namespace Novell.Directory.Ldap.NETStandard.FunctionalTests
{
    public sealed class ConnectWithClientCertificateTests : IDisposable
    {
        private readonly X509Certificate2 _x509Certificate2;
        private readonly LdapConnectionOptions _ldapConnectionOptions;
        private readonly string _expectedAuthzId;

        public ConnectWithClientCertificateTests()
        {
            _expectedAuthzId = "dn:cn=external-test,dc=example,dc=com";
#pragma warning disable SYSLIB0057
            _x509Certificate2 = new X509Certificate2(
                CertsTestHelper.GetCertificate("external-test.pfx"),
                "password");
#pragma warning restore SYSLIB0057
            _ldapConnectionOptions = new LdapConnectionOptions()
                .ConfigureIpAddressFilter(ipAddress => ipAddress.AddressFamily == AddressFamily.InterNetwork)
                .ConfigureRemoteCertificateValidationCallback((sender, certificate, chain, errors) => true)
                .ConfigureLocalCertificateSelectionCallback(LdapConnectionLocalCertificateSelectionCallback)
                .ConfigureClientCertificates(new List<X509Certificate>()
                {
                    _x509Certificate2,
                });
        }

        [Fact]
        public async Task Bind_with_client_certificate_is_successful()
        {
            _ldapConnectionOptions.UseSsl();
            using var ldapConnection = new LdapConnection(_ldapConnectionOptions);

            await ldapConnection.ConnectAsync(TestsConfig.LdapServer.ServerAddress, TestsConfig.LdapServer.ServerPortSsl);

            await ldapConnection.BindAsync(new SaslExternalRequest());

            Assert.True(ldapConnection.Bound);
            var response = await ldapConnection.WhoAmIAsync();
            Assert.Equal(_expectedAuthzId, response.AuthzId);
        }

        [Fact]
        public async Task Bind_with_client_certificate_using_start_tls_is_successful()
        {
            using var ldapConnection = new LdapConnection(_ldapConnectionOptions);
            await ldapConnection.ConnectAsync(TestsConfig.LdapServer.ServerAddress, TestsConfig.LdapServer.ServerPort);

            try
            {
                await ldapConnection.StartTlsAsync();

                await ldapConnection.BindAsync(new SaslExternalRequest());

                Assert.True(ldapConnection.Bound);
                var response = await ldapConnection.WhoAmIAsync();
                Assert.Equal(_expectedAuthzId, response.AuthzId);
            }
            finally
            {
                await ldapConnection.StopTlsAsync();
            }
        }

        [Fact]
        public async Task Bind_with_client_certificate_using_obsoleted_api_is_successful()
        {
            var ldapConnectionOptions = new LdapConnectionOptions()
                .UseSsl()
                .ConfigureIpAddressFilter(ipAddress => ipAddress.AddressFamily == AddressFamily.InterNetwork)
                .ConfigureClientCertificates(new List<X509Certificate>()
                {
                    _x509Certificate2,
                });
            using var ldapConnection = new LdapConnection(ldapConnectionOptions);
#pragma warning disable CS0618 // Type or member is obsolete
            ldapConnection.UserDefinedServerCertValidationDelegate += LdapConnectionUserDefinedServerCertValidationDelegate;
            ldapConnection.UserDefinedClientCertSelectionDelegate += LdapConnectionLocalCertificateSelectionCallback;
#pragma warning restore CS0618 // Type or member is obsolete

            await ldapConnection.ConnectAsync(TestsConfig.LdapServer.ServerAddress, TestsConfig.LdapServer.ServerPortSsl);

            await ldapConnection.BindAsync(new SaslExternalRequest());

            Assert.True(ldapConnection.Bound);
            var response = await ldapConnection.WhoAmIAsync();
            Assert.Equal(_expectedAuthzId, response.AuthzId);
        }

        private bool LdapConnectionUserDefinedServerCertValidationDelegate(object sender, X509Certificate certificate, X509Chain chain, System.Net.Security.SslPolicyErrors sslPolicyErrors)
        {
            return true;
        }

        private X509Certificate LdapConnectionLocalCertificateSelectionCallback(object sender, string targetHost, X509CertificateCollection localCertificates, X509Certificate remoteCertificate, string[] acceptableIssuers)
        {
            return _x509Certificate2;
        }

        public void Dispose()
        {
            _x509Certificate2?.Dispose();
        }
    }
}
