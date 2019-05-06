using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using Bogus;

namespace Notif.Email.Options.Fakes
{
    [ExcludeFromCodeCoverage]
    public static class MailSettingsConfigFake
    {
        private static readonly Faker<MailSettingsConfig> Faker =
            new Faker<MailSettingsConfig>()
                .RuleFor(e => e.Smtp, f => new MailSettingsConfig.SmtpConfig()
                {
                    Servers = SmtpServerConfigFake.Generate(1)
                });

        public static MailSettingsConfig Generate()
        {
            return Faker.Generate();
        }

        public static List<MailSettingsConfig> Generate(int count)
        {
            return Faker.Generate(count).ToList();
        }

        public static MailSettingsConfig HasServers(this MailSettingsConfig config, int serversCount)
        {
            config.Smtp.Servers = SmtpServerConfigFake.Generate(serversCount);
            return config;
        }
    }
}