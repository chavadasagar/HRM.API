using MasterPOS.API.Domain.Common.Events;

namespace MasterPOS.API.Application.Catalog.Counters;

public class UpdateCounterRequest : IRequest<Guid>
{
    public Guid Id { get; set; }
    public string Name { get; set; } = default!;
    public string? Description { get; set; }
    public bool IsDefault { get; set; }
    public bool IsActive { get; set; }
    public Guid StoreId { get; set; }
}

public class UpdateCounterRequestHandler : IRequestHandler<UpdateCounterRequest, Guid>
{
    private readonly IRepository<Counter> _repository;
    private readonly IStringLocalizer<UpdateCounterRequestHandler> _localizer;
    private readonly IFileStorageService _file;

    public UpdateCounterRequestHandler(IRepository<Counter> repository, IStringLocalizer<UpdateCounterRequestHandler> localizer, IFileStorageService file) =>
        (_repository, _localizer, _file) = (repository, localizer, file);

    public async Task<Guid> Handle(UpdateCounterRequest request, CancellationToken cancellationToken)
    {
        var counter = await _repository.GetByIdAsync(request.Id, cancellationToken);

        _ = counter ?? throw new NotFoundException(string.Format(_localizer["counter.notfound"], request.Id));

        var updatedCounter = counter.Update(request.Name, request.Description, request.StoreId, request.IsDefault, request.IsActive);

        // Add Domain Events to be raised after the commit
        counter.DomainEvents.Add(EntityUpdatedEvent.WithEntity(counter));

        await _repository.UpdateAsync(updatedCounter, cancellationToken);

        return request.Id;
    }
}