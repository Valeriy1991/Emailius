using System;
using System.Collections.Generic;
using System.Text;
using Notif.Email.Options;

namespace Notif.Email.Factories
{
    public class MailMessageCreateRequest
    {
        public List<string> RequiredRecipients { get; set; } = new List<string>();
        public SmtpServerConfig SmtpServerConfig { get; set; }
        public EmailMessage EmailMessage { get; set; }
        public bool IsReadSenderFromConfig { get; set; }
    }
}