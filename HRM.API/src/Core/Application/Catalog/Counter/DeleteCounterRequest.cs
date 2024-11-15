using MasterPOS.API.Domain.Common.Events;

namespace MasterPOS.API.Application.Catalog.Counters;

public class DeleteCounterRequest : IRequest<Guid>
{
    public Guid Id { get; set; }

    public DeleteCounterRequest(Guid id) => Id = id;
}

public class DeleteCounterRequestHandler : IRequestHandler<DeleteCounterRequest, Guid>
{
    private readonly IRepository<Counter> _repository;
    private readonly IStringLocalizer<DeleteCounterRequestHandler> _localizer;

    public DeleteCounterRequestHandler(IRepository<Counter> repository, IStringLocalizer<DeleteCounterRequestHandler> localizer) =>
        (_repository, _localizer) = (repository, localizer);

    public async Task<Guid> Handle(DeleteCounterRequest request, CancellationToken cancellationToken)
    {
        var counter = await _repository.GetByIdAsync(request.Id, cancellationToken);

        _ = counter ?? throw new NotFoundException(_localizer["counter.notfound"]);

        // Add Domain Events to be raised after the commit
        counter.DomainEvents.Add(EntityDeletedEvent.WithEntity(counter));

        await _repository.DeleteAsync(counter, cancellationToken);

        return request.Id;
    }
}