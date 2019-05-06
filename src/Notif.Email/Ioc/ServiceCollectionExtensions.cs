using System;
using System.Net.Mail;
using Microsoft.Extensions.DependencyInjection;
using Notif.Abstractions;
using Notif.Email.Factories;
using Notif.Email.Smtp;
using Notif.Email.Smtp.Abstract;

namespace Notif.Email.Ioc
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddNotifEmail(this IServiceCollection services)
        {
            return services
                .AddTransient<INotificator<EmailMessage>, EmailNotificator>()
                .AddSingleton<ISmtpClientFactory, SmtpClientWithoutSslFactory>()
                .AddSingleton<IMessageFactory<MailMessageCreateRequest, MailMessage>, MailMessageFactory>();
        }
    }
}