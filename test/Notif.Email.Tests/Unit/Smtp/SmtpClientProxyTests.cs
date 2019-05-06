using System;
using System.Diagnostics.CodeAnalysis;
using System.Net.Mail;
using Xunit;
using Bogus;
using Notif.Email.Smtp;

namespace Notif.Email.Tests.Unit.Smtp
{
    [ExcludeFromCodeCoverage]
    [Trait("Category", "Unit")]
    public class SmtpClientProxyTests : IDisposable
    {
        private readonly Faker _faker = new Faker();
        private readonly SmtpClientProxy _proxy;
        private readonly string _host;
        private readonly int _port;

        public SmtpClientProxyTests()
        {
            _host = "some-host.com";
            _port = 25;
            var smtpClient = new SmtpClient(_host, _port);
            _proxy = new SmtpClientProxy(smtpClient);
        }

        [Fact]
        public void Host_ReturnCorrectHostOfSmtpClient()
        {
            // Arrange
            // Act
            var host = _proxy.Host;
            // Assert
            Assert.Equal(_host, host);
        }

        public void Dispose()
        {
            _proxy?.Dispose();
        }
    }
}