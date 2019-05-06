using System.Collections.Generic;

namespace Notif.Email
{
    public class EmailMessage
    {
        public string From { get; set; }
        public List<string> To { get; } = new List<string>();
        public string Subject { get; set; }
        public string Body { get; set; }

        public EmailMessage(IEnumerable<string> toList, string subject, string body)
        {
            To.AddRange(toList);
            Subject = subject;
            Body = body;
        }

        public EmailMessage(string @from, IEnumerable<string> toList, string subject, string body)
        {
            From = from;
            To.AddRange(toList);
            Subject = subject;
            Body = body;
        }
    }
}