using System.Linq;
using System.Net.Mail;
using System.Threading.Tasks;
using Notif.Abstractions;

namespace Notif.Email.Factories
{
    public class MailMessageFactory : IMessageFactory<MailMessageCreateRequest, MailMessage>
    {
        public Task<MailMessage> CreateAsync(MailMessageCreateRequest createRequest)
        {
            var emailMessage = createRequest.EmailMessage;

            var message = new MailMessage()
            {
                IsBodyHtml = true,
                Body = emailMessage.Body,
                Subject = emailMessage.Subject,
            };

            if (createRequest.IsReadSenderFromConfig)
            {
                message.From = new MailAddress(createRequest.SmtpServerConfig.SenderAddress,
                    createRequest.SmtpServerConfig.SenderName);
            }
            else
            {
                message.From = new MailAddress(emailMessage.From);
            }

            foreach (var to in emailMessage.To)
            {
                message.To.Add(to);
            }

            var requiredRecipients = createRequest.RequiredRecipients;
            if (requiredRecipients.Any())
            {
                foreach (var requiredRecipient in requiredRecipients)
                {
                    message.To.Add(requiredRecipient);
                }
            }


            return Task.FromResult(message);
        }
    }
}