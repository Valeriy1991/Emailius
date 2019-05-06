using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using Bogus;

namespace Notif.Email.Options.Fakes
{
    [ExcludeFromCodeCoverage]
    public static class SmtpServerConfigFake
    {
        private static readonly Faker<SmtpServerConfig> Faker =
            new Faker<SmtpServerConfig>()
                .RuleFor(e => e.Host, f => f.Internet.Ip())
                .RuleFor(e => e.Port, f => f.Random.Int(min: 1, max: 99999))
                .RuleFor(e => e.Login, f => f.Internet.UserName())
                .RuleFor(e => e.Password, f => f.Internet.Password())
                .RuleFor(e => e.SenderName, f => f.Internet.Email())
                .RuleFor(e => e.SenderAddress, f => f.Internet.Email());

        public static SmtpServerConfig Generate()
        {
            return Faker.Generate();
        }

        public static List<SmtpServerConfig> Generate(int count)
        {
            return Faker.Generate(count).ToList();
        }

        public static SmtpServerConfig HasHost(this SmtpServerConfig config, string host)
        {
            config.Host = host;
            return config;
        }

        public static SmtpServerConfig HasPort(this SmtpServerConfig config, int port)
        {
            config.Port = port;
            return config;
        }

        public static SmtpServerConfig HasLogin(this SmtpServerConfig config, string login)
        {
            config.Login = login;
            return config;
        }

        public static SmtpServerConfig HasPassword(this SmtpServerConfig config, string password)
        {
            config.Password = password;
            return config;
        }

        public static SmtpServerConfig HasSenderName(this SmtpServerConfig config, string senderName)
        {
            config.SenderName = senderName;
            return config;
        }

        public static SmtpServerConfig HasSenderAddress(this SmtpServerConfig config, string senderAddress)
        {
            config.SenderAddress = senderAddress;
            return config;
        }
    }
}