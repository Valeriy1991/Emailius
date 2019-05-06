using Notif.Email.Options;

namespace Notif.Email.Smtp.Abstract
{
    public interface ISmtpClientFactory
    {
        ISmtpClient Create(SmtpServerConfig serverConfig);
    }
}
