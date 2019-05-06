using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;
using Bogus;
using Notif.Email.Factories;
using Notif.Email.Options;
using Notif.Email.Smtp;
using Xunit.Abstractions;

namespace Notif.Email.Tests.Integration
{
    [ExcludeFromCodeCoverage]
    [Trait("Category", "Integration - Email")]
    public class EmailNotificatorTests
    {
        private readonly Faker _faker = new Faker();
        private readonly ITestOutputHelper _output;
        private readonly EmailNotificator _notificator;
        private readonly EmailMessage _emailMessage;
        
        public EmailNotificatorTests(ITestOutputHelper output)
        {
            _output = output;
            var senderAddress = "notif-test@test.com";
            var senderName = "notif-test";
            var mailhogSmtpServerConfig = new SmtpServerConfig()
            {
                Host = "localhost",
                Port = 25,
                SenderName = senderName,
                SenderAddress = senderAddress
            };
            var mailSettingsConfig = new MailSettingsConfig()
            {
                Smtp = new MailSettingsConfig.SmtpConfig()
                {
                    Servers = new List<SmtpServerConfig>()
                    {
                        mailhogSmtpServerConfig
                    }
                }
            };

            var toList = new List<string>();
            var subject = "Test subject";
            var body = "Test body";

            _emailMessage = new EmailMessage(toList, subject, body);

            var smtpClientFactory = new SmtpClientWithoutSslFactory();
            var mailMessageFactory = new MailMessageFactory();
            _notificator = new EmailNotificator(smtpClientFactory, mailSettingsConfig, mailMessageFactory);
        }

        [Fact]
        public async Task NotifyAsync_ReturnSuccess()
        {
            // Arrange
            var to = _faker.Internet.Email();
            _emailMessage.To.Add(to);
            // Act
            var result = await _notificator.NotifyAsync(_emailMessage);
            // Assert
            _output.WriteLine(result.ToString());
            Assert.True(result.Success);
        }

        [Fact]
        public async Task NotifyAsync_EmailWasReallySent()
        {
            // Arrange
            var to = _faker.Internet.Email();
            _emailMessage.To.Add(to);
            // Act
            await _notificator.NotifyAsync(_emailMessage);
            // Assert
            var emailWasReallySent = await CheckEmailInMailhog(to);
            Assert.True(emailWasReallySent);
        }

        private async Task<bool> CheckEmailInMailhog(string to)
        {
            var httpClient = new HttpClient(new HttpClientHandler());
            var encodedTo = WebUtility.UrlEncode(to);
            var mailhogHost = "localhost:8025";
            var urlToMailhog =
                $"http://{mailhogHost}/api/v2/search?kind=to&query={encodedTo}";

            var response = await httpClient.GetAsync(urlToMailhog);
            var content = await response.Content.ReadAsStringAsync();
            return content.Contains($"\"To\":[\"{to}\"]");
        }
    }
}