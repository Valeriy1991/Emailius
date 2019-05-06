using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Threading.Tasks;
using Ether.Outcomes;
using Notif.Abstractions;
using Notif.Email.Factories;
using Notif.Email.Options;
using Notif.Email.Smtp.Abstract;

namespace Notif.Email
{
    public class EmailNotificator : INotificator<EmailMessage>
    {
        private readonly ISmtpClientFactory _smtpClientFactory;
        private readonly MailSettingsConfig _mailSettingsConfig;
        private readonly IMessageFactory<MailMessageCreateRequest, MailMessage> _mailMessageFactory;

        public EmailNotificator(ISmtpClientFactory smtpClientFactory,
            MailSettingsConfig mailSettingsConfig,
            IMessageFactory<MailMessageCreateRequest, MailMessage> mailMessageFactory)
        {
            _smtpClientFactory = smtpClientFactory;
            _mailSettingsConfig = mailSettingsConfig;
            _mailMessageFactory = mailMessageFactory;
        }

        public Task<IOutcome> NotifyAsync(EmailMessage message)
        {
            try
            {
                return SendOverMultipleSmtpServersAsync(message);
            }
            catch (Exception ex)
            {
                return Task.FromResult((IOutcome) Outcomes.Failure().WithMessage(ex.Message));
            }
        }

        private async Task<IOutcome> SendOverMultipleSmtpServersAsync(EmailMessage message)
        {
            var errors = new List<string>();
            var smtpServerConfigurations = _mailSettingsConfig.Smtp.Servers;
            var wasSentSuccessfully = false;

            foreach (var smtpServerConfiguration in smtpServerConfigurations)
            {
                try
                {
                    if (wasSentSuccessfully)
                        continue;

                    var createRequest = new MailMessageCreateRequest()
                    {
                        EmailMessage = message,
                        SmtpServerConfig = smtpServerConfiguration,
                        IsReadSenderFromConfig = true,
                        RequiredRecipients = _mailSettingsConfig.Smtp.RequiredRecipients,
                    };
                    var mailMessage = await _mailMessageFactory.CreateAsync(createRequest);

                    using (var smtpClient = _smtpClientFactory.Create(smtpServerConfiguration))
                    {
                        await smtpClient.SendAsync(mailMessage);
                        wasSentSuccessfully = true;
                    }
                }
                catch (Exception ex)
                {
                    wasSentSuccessfully = false;

                    var error = ex.Message;
                    if (ex.InnerException != null)
                    {
                        var innerEx = ex.InnerException;
                        while (innerEx.InnerException != null)
                        {
                            innerEx = innerEx.InnerException;
                        }

                        error = innerEx.Message;
                    }

                    errors.Add($"SMTP {smtpServerConfiguration.Host} throw an error: {error}");
                }
            }

            if (wasSentSuccessfully)
                return Outcomes.Success();

            return Outcomes.Failure().WithMessagesFrom(errors.Distinct());
        }
    }
}