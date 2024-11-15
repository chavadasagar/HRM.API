using MasterPOS.API.Domain.Common.Events;

namespace MasterPOS.API.Application.Catalog.Counters;

public class CreateCounterRequest : IRequest<Guid>
{
    public string Name { get; set; } = default!;
    public string? Description { get; set; }
    public bool IsDefault { get; set; }
    public bool IsActive { get; set; }
    public Guid StoreId { get; set; }
}

public class CreateCounterRequestHandler : IRequestHandler<CreateCounterRequest, Guid>
{
    private readonly IRepository<Counter> _repository;
    private readonly IFileStorageService _file;

    public CreateCounterRequestHandler(IRepository<Counter> repository, IFileStorageService file) =>
        (_repository, _file) = (repository, file);

    public async Task<Guid> Handle(CreateCounterRequest request, CancellationToken cancellationToken)
    {
        var counter = new Counter(request.Name, request.Description, request.StoreId, request.IsDefault, request.IsActive);

        // Add Domain Events to be raised after the commit
        counter.DomainEvents.Add(EntityCreatedEvent.WithEntity(counter));

        await _repository.AddAsync(counter, cancellationToken);

        return counter.Id;
    }
}