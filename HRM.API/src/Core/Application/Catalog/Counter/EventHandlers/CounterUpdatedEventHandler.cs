using MasterPOS.API.Domain.Common.Events;

namespace MasterPOS.API.Application.Catalog.Counters.EventHandlers;

public class CounterUpdatedEventHandler : EventNotificationHandler<EntityUpdatedEvent<Counter>>
{
    private readonly ILogger<CounterUpdatedEventHandler> _logger;

    public CounterUpdatedEventHandler(ILogger<CounterUpdatedEventHandler> logger) => _logger = logger;

    public override Task Handle(EntityUpdatedEvent<Counter> @event, CancellationToken cancellationToken)
    {
        _logger.LogInformation("{event} Triggered", @event.GetType().Name);
        return Task.CompletedTask;
    }
}