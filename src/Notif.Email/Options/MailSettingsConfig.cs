using System;
using System.Collections.Generic;

namespace Notif.Email.Options
{
    public class MailSettingsConfig
    {
        public virtual SmtpConfig Smtp { get; set; } = new SmtpConfig();

        public class SmtpConfig
        {
            public List<string> RequiredRecipients { get; set; } = new List<string>();
            public List<SmtpServerConfig> Servers { get; set; } = new List<SmtpServerConfig>();
        }
    }
}