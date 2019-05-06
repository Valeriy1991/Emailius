using System;
using System.Net.Mail;
using System.Threading.Tasks;

namespace Notif.Email.Smtp.Abstract
{
    public interface ISmtpClient : IDisposable
    {
        string Host { get; }
        bool IsSslEnabled { get; }
        bool UseDefaultCredentials { get; }
        SmtpDeliveryMethod DeliveryMethod { get; }

        Task SendAsync(MailMessage mailMessage);
    }
}