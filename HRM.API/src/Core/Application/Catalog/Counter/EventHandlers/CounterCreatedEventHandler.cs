using HRM.API.Domain.Common.Events;

namespace HRM.API.Application.Catalog.Counters.EventHandlers;

public class CounterCreatedEventHandler : EventNotificationHandler<EntityCreatedEvent<Counter>>
{
    private readonly ILogger<CounterCreatedEventHandler> _logger;

    public CounterCreatedEventHandler(ILogger<CounterCreatedEventHandler> logger) => _logger = logger;

    public override Task Handle(EntityCreatedEvent<Counter> @event, CancellationToken cancellationToken)
    {
        _logger.LogInformation("{event} Triggered", @event.GetType().Name);
        return Task.CompletedTask;
    }
}