using System.Threading.Tasks;
using Notif.Abstractions;
using Notif.Email;

namespace ConsoleApp.ServiceProvider
{
    public class ExampleEmailMessageFactory : IMessageFactory<object, EmailMessage>
    {
        public Task<EmailMessage> CreateAsync(object createRequest)
        {
            var emailRecipients = new[] {"console-app@test.com"};
            var subject = "Hello...";
            var body = "... world";
            var emailMessage = new EmailMessage(emailRecipients, subject, body);

            return Task.FromResult(emailMessage);
        }
    }
}