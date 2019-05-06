using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Net.Mail;
using System.Threading.Tasks;
using Xunit;
using Bogus;
using Notif.Abstractions;
using Notif.Email.Factories;
using Notif.Email.Options;
using Notif.Email.Options.Fakes;
using Notif.Email.Smtp.Abstract;
using NSubstitute;
using NSubstitute.ExceptionExtensions;

namespace Notif.Email.Tests.Unit
{
    [ExcludeFromCodeCoverage]
    [Trait("Category", "Unit")]
    public class EmailNotificatorTests
    {
        private readonly Faker _faker = new Faker();
        private readonly ISmtpClientFactory _smtpClientFactory;
        private readonly EmailNotificator _notificator;
        private readonly MailSettingsConfig _mailSettingsConfig;
        private readonly ISmtpClient _smtpClient1;
        private readonly ISmtpClient _smtpClient2;
        private readonly ISmtpClient _smtpClient3;
        private readonly EmailMessage _emailMessage;
        private IMessageFactory<MailMessageCreateRequest, MailMessage> _mailMessageFactory;

        public EmailNotificatorTests()
        {
            _smtpClientFactory = Substitute.For<ISmtpClientFactory>();
            _mailSettingsConfig = MailSettingsConfigFake.Generate();

            _mailMessageFactory = Substitute.For<IMessageFactory<MailMessageCreateRequest, MailMessage>>();
            _notificator = new EmailNotificator(_smtpClientFactory, _mailSettingsConfig, _mailMessageFactory);

            var recipients = new List<string>()
            {
                _faker.Internet.Email(),
            };
            _emailMessage = new EmailMessage("from", recipients, "subject", "body");

            var smtpServerConfig1 = SmtpServerConfigFake.Generate();
            var smtpServerConfig2 = SmtpServerConfigFake.Generate();
            var smtpServerConfig3 = SmtpServerConfigFake.Generate();
            _mailSettingsConfig.Smtp.Servers = new List<SmtpServerConfig>()
            {
                smtpServerConfig1, smtpServerConfig2, smtpServerConfig3
            };

            _smtpClient1 = Substitute.ForPartsOf<FakeSmtpClient>(smtpServerConfig1.Host);
            _smtpClient2 = Substitute.ForPartsOf<FakeSmtpClient>(smtpServerConfig2.Host);
            _smtpClient3 = Substitute.ForPartsOf<FakeSmtpClient>(smtpServerConfig3.Host);

            _smtpClientFactory.Create(smtpServerConfig1).Returns(_smtpClient1);
            _smtpClientFactory.Create(smtpServerConfig2).Returns(_smtpClient2);
            _smtpClientFactory.Create(smtpServerConfig3).Returns(_smtpClient3);
        }

        [Fact]
        public async Task NotifyAsync_ConfigHas3SmtpServersAndFirstOfThemDoesNotWork_SmtpClient1WasReceived()
        {
            // Arrange
            _smtpClient1.SendAsync(Arg.Any<MailMessage>()).Throws(new Exception());
            // Act
            await _notificator.NotifyAsync(_emailMessage);
            // Assert
            await _smtpClient1.Received(1).SendAsync(Arg.Any<MailMessage>());
        }

        [Fact]
        public async Task NotifyAsync_ConfigHas3SmtpServersAndFirstOfThemDoesNotWork_SmtpClient2WasReceived()
        {
            // Arrange
            _smtpClient1.SendAsync(Arg.Any<MailMessage>()).Throws(new Exception());
            // Act
            await _notificator.NotifyAsync(_emailMessage);
            // Assert
            await _smtpClient2.Received(1).SendAsync(Arg.Any<MailMessage>());
        }

        [Fact]
        public async Task NotifyAsync_ConfigHas3SmtpServersAndFirstOfThemDoesNotWork_SmtpClient3WasNotReceived()
        {
            // Arrange
            _smtpClient1.SendAsync(Arg.Any<MailMessage>()).Throws(new Exception());
            // Act
            await _notificator.NotifyAsync(_emailMessage);
            // Assert
            await _smtpClient3.Received(0).SendAsync(Arg.Any<MailMessage>());
        }

        [Fact]
        public async Task NotifyAsync_AllSmtpServersDoesNotWork_ReturnFailureWithCorrectMessage()
        {
            // Arrange
            var error1 = "error 1";
            var error2 = "error 2";
            var error3 = "error 3";
            _smtpClient1.SendAsync(Arg.Any<MailMessage>()).Throws(new Exception(error1));
            _smtpClient2.SendAsync(Arg.Any<MailMessage>()).Throws(new Exception(error2));
            _smtpClient3.SendAsync(Arg.Any<MailMessage>()).Throws(new Exception(error3));
            // Act
            var result = await _notificator.NotifyAsync(_emailMessage);
            // Assert
            Assert.True(result.Failure);
            Assert.Contains($"SMTP {_smtpClient1.Host} throw an error: {error1}", result.ToString());
            Assert.Contains($"SMTP {_smtpClient2.Host} throw an error: {error2}", result.ToString());
            Assert.Contains($"SMTP {_smtpClient3.Host} throw an error: {error3}", result.ToString());
        }

        [Fact]
        public async Task NotifyAsync_CreateRequestForMailMessageFactoryHasIsReadSenderFromConfigAsTrue()
        {
            // Arrange
            // Act
            await _notificator.NotifyAsync(_emailMessage);
            // Assert
            await _mailMessageFactory.Received(1).CreateAsync(
                Arg.Is<MailMessageCreateRequest>(request => request.IsReadSenderFromConfig));
        }

        [Fact]
        public async Task NotifyAsync_CreateRequestForMailMessageFactoryHasNotNullableSmtpServerConfig()
        {
            // Arrange
            // Act
            await _notificator.NotifyAsync(_emailMessage);
            // Assert
            await _mailMessageFactory.Received(1).CreateAsync(
                Arg.Is<MailMessageCreateRequest>(request => request.SmtpServerConfig != null));
        }

        [Fact]
        public async Task NotifyAsync_CreateRequestForMailMessageFactoryHasNotNullableEmailMessage()
        {
            // Arrange
            // Act
            await _notificator.NotifyAsync(_emailMessage);
            // Assert
            await _mailMessageFactory.Received(1).CreateAsync(
                Arg.Is<MailMessageCreateRequest>(request => request.EmailMessage != null));
        }

        [Fact]
        public async Task NotifyAsync_CreateRequestForMailMessageFactoryHasCorrectRequiredRecipients()
        {
            // Arrange
            var requiredRecipients = new List<string>()
            {
                _faker.Internet.Email(),
                _faker.Internet.Email(),
                _faker.Internet.Email(),
            };
            _mailSettingsConfig.Smtp.RequiredRecipients.AddRange(requiredRecipients);
            // Act
            await _notificator.NotifyAsync(_emailMessage);
            // Assert
            await _mailMessageFactory.Received(1).CreateAsync(
                Arg.Is<MailMessageCreateRequest>(request =>
                    request.RequiredRecipients.Intersect(requiredRecipients).Count() == requiredRecipients.Count));
        }
    }
}