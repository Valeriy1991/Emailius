using System;
using System.Net.Mail;
using System.Threading.Tasks;
using Notif.Email.Smtp.Abstract;

namespace Notif.Email.Smtp
{
    public class SmtpClientProxy : ISmtpClient
    {
        private readonly SmtpClient _smtpClient;

        public SmtpClientProxy(SmtpClient smtpClient)
        {
            _smtpClient = smtpClient ?? throw new ArgumentNullException(nameof(smtpClient));
        }

        public string Host => _smtpClient.Host;
        public bool IsSslEnabled => _smtpClient.EnableSsl;
        public bool UseDefaultCredentials => _smtpClient.UseDefaultCredentials;
        public SmtpDeliveryMethod DeliveryMethod => _smtpClient.DeliveryMethod;

        public Task SendAsync(MailMessage mailMessage)
        {
            return _smtpClient.SendMailAsync(mailMessage);
        }

        public void Dispose()
        {
            _smtpClient?.Dispose();
        }
    }
}