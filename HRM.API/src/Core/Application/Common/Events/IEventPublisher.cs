using HRM.API.Shared.Events;

namespace HRM.API.Application.Common.Events;

public interface IEventPublisher : ITransientService
{
    Task PublishAsync(IEvent @event);
}