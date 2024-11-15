using MasterPOS.API.Shared.Events;

namespace MasterPOS.API.Application.Common.Events;

public interface IEventPublisher : ITransientService
{
    Task PublishAsync(IEvent @event);
}