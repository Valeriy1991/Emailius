using System.Threading.Tasks;

namespace Notif.Abstractions
{
    public interface IMessageTemplateReader
    {
        Task<string> ReadAllTextAsync(string templateFilePath);
    }
}