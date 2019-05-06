using System.Diagnostics.CodeAnalysis;
using Xunit;
using Bogus;
using Notif.Email.Options;

namespace Notif.Email.Tests.Unit.Options
{
    [ExcludeFromCodeCoverage]
    [Trait("Category", "Unit")]
    public class MailSettingsConfigTests
    {
        private readonly MailSettingsConfig _config;

        public MailSettingsConfigTests()
        {
            _config = new MailSettingsConfig();
        }

        [Fact]
        public void Ctor_SmtpIsNotNull()
        {
            // Arrange
            // Act
            var smtp = _config.Smtp;
            // Assert
            Assert.NotNull(smtp);
        }

        [Fact]
        public void Ctor_SmtpRequiredRecipientsIsEmpty()
        {
            // Arrange
            // Act
            var requiredRecipients = _config.Smtp.RequiredRecipients;
            // Assert
            Assert.Empty(requiredRecipients);
        }

        [Fact]
        public void Ctor_SmtpServersIsEmpty()
        {
            // Arrange
            // Act
            var servers = _config.Smtp.Servers;
            // Assert
            Assert.Empty(servers);
        }
    }
}