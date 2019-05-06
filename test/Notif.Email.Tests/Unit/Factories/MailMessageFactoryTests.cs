using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using Xunit;
using Bogus;
using Notif.Email.Factories;
using Notif.Email.Options;
using Notif.Email.Options.Fakes;

namespace Notif.Email.Tests.Unit.Factories
{
    [ExcludeFromCodeCoverage]
    [Trait("Category", "Unit")]
    public class MailMessageFactoryTests
    {
        private readonly Faker _faker = new Faker();
        private readonly MailMessageFactory _factory;
        private readonly SmtpServerConfig _smtpServerConfig;
        private readonly EmailMessage _emailMessage;
        private readonly MailMessageCreateRequest _request;

        public MailMessageFactoryTests()
        {
            _factory = new MailMessageFactory();

            _smtpServerConfig = SmtpServerConfigFake.Generate();

            var from = _faker.Internet.Email();
            var to = new List<string>()
            {
                _faker.Internet.Email()
            };
            _emailMessage = new EmailMessage(from, to, "subject", "body");
            _request = new MailMessageCreateRequest()
            {
                EmailMessage = _emailMessage,
                SmtpServerConfig = _smtpServerConfig,
                IsReadSenderFromConfig = false,
            };
        }

        [Fact]
        public async Task CreateAsync_ReturnNotNull()
        {
            // Arrange
            // Act
            var message = await _factory.CreateAsync(_request);
            // Assert
            Assert.NotNull(message);
        }

        [Fact]
        public async Task CreateAsync_ReturnedMailMessageHasIsBodyHtmlAsTrue()
        {
            // Arrange
            // Act
            var message = await _factory.CreateAsync(_request);
            // Assert
            Assert.True(message.IsBodyHtml);
        }

        [Fact]
        public async Task CreateAsync_IsReadSenderFromConfigIsTrue_ReturnedMailMessageHasFromAddressEqualsSenderAddressFromConfig()
        {
            // Arrange
            _request.IsReadSenderFromConfig = true;

            var senderEmail = _faker.Internet.Email();
            _smtpServerConfig.SenderAddress = senderEmail;
            // Act
            var message = await _factory.CreateAsync(_request);
            // Assert
            Assert.Equal(senderEmail, message.From.Address);
        }

        [Fact]
        public async Task CreateAsync_IsReadSenderFromConfigIsTrue_ReturnedMailMessageHasFromNameEqualsSenderAddressFromConfig()
        {
            // Arrange
            _request.IsReadSenderFromConfig = true;

            var senderName = _faker.Person.FullName;
            _smtpServerConfig.SenderName = senderName;
            // Act
            var message = await _factory.CreateAsync(_request);
            // Assert
            Assert.Equal(senderName, message.From.DisplayName);
        }

        [Fact]
        public async Task CreateAsync_IsReadSenderFromConfigIsFalse_EmailMessageFromIsNull_ThrowException()
        {
            // Arrange
            _request.EmailMessage.From = null;
            Func<Task> act = () => _factory.CreateAsync(_request);
            // Act
            var ex = await Record.ExceptionAsync(act);
            // Assert
            Assert.IsType<ArgumentNullException>(ex);
        }

        [Fact]
        public async Task CreateAsync_IsReadSenderFromConfigIsTrue_EmailMessageFromIsNull_DoesNotThrowException()
        {
            // Arrange
            _request.IsReadSenderFromConfig = true;
            _request.EmailMessage.From = null;
            Func<Task> act = () => _factory.CreateAsync(_request);
            // Act
            var ex = await Record.ExceptionAsync(act);
            // Assert
            Assert.Null(ex);
        }

        [Fact]
        public async Task CreateAsync_MailMessageHasAllRecipientsFromEmailMessage()
        {
            // Arrange
            var from = _faker.Internet.Email();
            var toList = new List<string>()
            {
                _faker.Internet.Email(),
                _faker.Internet.Email(),
                _faker.Internet.Email(),
                _faker.Internet.Email(),
            };
            var emailMessage = new EmailMessage(from, toList, "subject", "body");
            _request.EmailMessage = emailMessage;
            // Act
            var message = await _factory.CreateAsync(_request);
            // Assert
            Assert.Equal(toList.Count, message.To.Count);
        }

        [Fact]
        public async Task CreateAsync_CreateRequestHasSomeRequiredRecipients_ReturnedMailMessageHasThisRecipients()
        {
            // Arrange
            var requiredRecipient1 = _faker.Internet.Email();
            var requiredRecipient2 = _faker.Internet.Email();
            var requiredRecipients = new List<string>()
            {
                requiredRecipient1, requiredRecipient2
            };
            _request.RequiredRecipients.AddRange(requiredRecipients);
            // Act
            var message = await _factory.CreateAsync(_request);
            // Assert
            Assert.Equal(requiredRecipients.Count + 1, message.To.Count);
            Assert.Contains(message.To, e => e.Address == requiredRecipient1);
            Assert.Contains(message.To, e => e.Address == requiredRecipient2);
        }
    }
}