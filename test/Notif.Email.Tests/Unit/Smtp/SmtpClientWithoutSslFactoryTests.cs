using System.Diagnostics.CodeAnalysis;
using System.Net.Mail;
using Xunit;
using Bogus;
using Notif.Email.Options;
using Notif.Email.Smtp;

namespace Notif.Email.Tests.Unit.Smtp
{
    [ExcludeFromCodeCoverage]
    [Trait("Category", "Unit")]
    public class SmtpClientWithoutSslFactoryTests
    {
        private readonly Faker _faker = new Faker();
        private readonly SmtpClientWithoutSslFactory _factory;
        private readonly SmtpServerConfig _smtpConfig;

        public SmtpClientWithoutSslFactoryTests()
        {
            _factory = new SmtpClientWithoutSslFactory();
            _smtpConfig = new SmtpServerConfig();
        }

        [Fact]
        public void Create_ReturnedSmtpClientHasDisabledSsl()
        {
            // Arrange
            // Act
            var smtpClient = _factory.Create(_smtpConfig);
            // Assert
            Assert.False(smtpClient.IsSslEnabled);
        }

        [Fact]
        public void Create_ReturnedSmtpClientHasCorrectDeliveryMethod()
        {
            // Arrange
            var correctSmtpDeliveryMethod = SmtpDeliveryMethod.Network;
            // Act
            var smtpClient = _factory.Create(_smtpConfig);
            // Assert
            Assert.Equal(correctSmtpDeliveryMethod, smtpClient.DeliveryMethod);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public void Create_SmtpServerConfigHasNotLogin_ReturnedSmtpClientHasDefaultCredentials(string login)
        {
            // Arrange
            _smtpConfig.Login = login;
            // Act
            var smtpClient = _factory.Create(_smtpConfig);
            // Assert
            Assert.True(smtpClient.UseDefaultCredentials);
        }
        
        [Fact]
        public void Create_SmtpServerConfigHasLoginAndPassword_ReturnedSmtpClientHasNotDefaultCredentials()
        {
            // Arrange
            _smtpConfig.Login = "some-login";
            _smtpConfig.Password = "some-password";
            // Act
            var smtpClient = _factory.Create(_smtpConfig);
            // Assert
            Assert.False(smtpClient.UseDefaultCredentials);
        }
    }
}