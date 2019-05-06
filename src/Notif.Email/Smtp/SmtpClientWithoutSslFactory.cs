using System.Net;
using System.Net.Mail;
using Notif.Email.Options;
using Notif.Email.Smtp.Abstract;

namespace Notif.Email.Smtp
{
    public class SmtpClientWithoutSslFactory : ISmtpClientFactory
    {
        public ISmtpClient Create(SmtpServerConfig serverConfig)
        {
            var smtpHost = serverConfig.Host;
            var smtpPort = serverConfig.Port;

            var smtpClient = new SmtpClient(smtpHost, smtpPort)
            {
                DeliveryMethod = SmtpDeliveryMethod.Network,
                EnableSsl = false
            };

            if (string.IsNullOrWhiteSpace(serverConfig.Login))
            {
                smtpClient.UseDefaultCredentials = true;
            }
            else
            {
                smtpClient.Credentials = new NetworkCredential(serverConfig.Login, serverConfig.Password);
            }

            return new SmtpClientProxy(smtpClient);
        }
    }
}