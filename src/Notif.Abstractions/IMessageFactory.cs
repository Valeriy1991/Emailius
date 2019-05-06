using System.Threading.Tasks;

namespace Notif.Abstractions
{
    public interface IMessageFactory<TRequest, TMessage>
    {
        Task<TMessage> CreateAsync(TRequest createRequest);
    }
}