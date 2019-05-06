using System.Diagnostics.CodeAnalysis;
using Xunit;
using Bogus;
using Notif.Email.Options;

namespace Notif.Email.Tests.Unit.Options
{
    [ExcludeFromCodeCoverage]
    [Trait("Category", "Unit")]
    public class SmtpServerConfigTests
    {
        private readonly Faker _faker = new Faker();
        private readonly SmtpServerConfig _config;

        public SmtpServerConfigTests()
        {
            _config = new SmtpServerConfig();
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public void Ctor_SenderNameIsNullOrEmpty_IsSenderNameSubmittedReturnFalse(string senderName)
        {
            // Arrange
            _config.SenderName = senderName;
            // Act
            var isSenderNameSubmitted = _config.IsSenderNameSubmitted;
            // Assert
            Assert.False(isSenderNameSubmitted);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public void Ctor_SenderAddressIsNullOrEmpty_IsSenderAddressSubmitted(string senderAddress)
        {
            // Arrange
            _config.SenderAddress = senderAddress;
            // Act
            var isSenderAddressSubmitted = _config.IsSenderAddressSubmitted;
            // Assert
            Assert.False(isSenderAddressSubmitted);
        }
    }
}