using System.Threading.Tasks;
using Ether.Outcomes;

namespace Notif.Abstractions
{
    public interface INotificator<TMessage>
    {
        Task<IOutcome> NotifyAsync(TMessage message);
    }
}
