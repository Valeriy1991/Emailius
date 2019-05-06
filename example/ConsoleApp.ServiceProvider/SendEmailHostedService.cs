using System;
using System.Net.Mail;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Notif.Abstractions;
using Notif.Email;
using Notif.Email.Factories;

namespace ConsoleApp.ServiceProvider
{
    public class SendEmailHostedService : IHostedService, IDisposable
    {
        private readonly ILogger _logger;
        private readonly INotificator<EmailMessage> _notificator;
        private readonly IMessageFactory<object, EmailMessage> _emailMessageFactory;

        public SendEmailHostedService(
            ILogger<SendEmailHostedService> logger,
            INotificator<EmailMessage> notificator,
            IMessageFactory<object, EmailMessage> emailMessageFactory)
        {
            _logger = logger;
            _notificator = notificator;
            _emailMessageFactory = emailMessageFactory;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogTrace("Starting send email...");

            var emailMessage = await _emailMessageFactory.CreateAsync(null);
            var sendResult = await _notificator.NotifyAsync(emailMessage);
            _logger.LogInformation($"Email sent result:\n\n{sendResult}");
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogTrace("Stopping");
            return Task.CompletedTask;
        }

        public void Dispose()
        {

        }
    }
}