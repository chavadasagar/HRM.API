using MasterPOS.API.Domain.Common.Events;

namespace MasterPOS.API.Application.Catalog.Counters.EventHandlers;

public class CounterDeletedEventHandler : EventNotificationHandler<EntityDeletedEvent<Counter>>
{
    private readonly ILogger<CounterDeletedEventHandler> _logger;

    public CounterDeletedEventHandler(ILogger<CounterDeletedEventHandler> logger) => _logger = logger;

    public override Task Handle(EntityDeletedEvent<Counter> @event, CancellationToken cancellationToken)
    {
        _logger.LogInformation("{event} Triggered", @event.GetType().Name);
        return Task.CompletedTask;
    }
}