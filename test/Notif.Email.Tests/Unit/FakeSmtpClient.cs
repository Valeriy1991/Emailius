using System.Diagnostics.CodeAnalysis;
using System.Net.Mail;
using System.Threading.Tasks;
using Notif.Email.Smtp.Abstract;

namespace Notif.Email.Tests.Unit
{
    [ExcludeFromCodeCoverage]
    public class FakeSmtpClient : ISmtpClient
    {
        public FakeSmtpClient(string host)
        {
            Host = host;
        }

        public void Dispose()
        {

        }

        public string Host { get; }
        public bool IsSslEnabled => false;
        public bool UseDefaultCredentials => true;
        public SmtpDeliveryMethod DeliveryMethod => SmtpDeliveryMethod.Network;

        public virtual Task SendAsync(MailMessage mailMessage)
        {
            return Task.CompletedTask;
        }
    }
}