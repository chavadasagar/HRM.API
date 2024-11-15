namespace MasterPOS.API.Application.Catalog.Counters;
public class UpdateStatusCounterRequest : IRequest<Guid>
{
    public Guid Id { get; set; }
    public bool IsActive { get; set; }
}

public class UpdateStatusCounterRequestHandler : IRequestHandler<UpdateStatusCounterRequest, Guid>
{
    private readonly IRepositoryWithEvents<Counter> _repository;
    private readonly IStringLocalizer<UpdateStatusCounterRequestHandler> _localizer;

    public UpdateStatusCounterRequestHandler(IRepositoryWithEvents<Counter> repository, IStringLocalizer<UpdateStatusCounterRequestHandler> localizer) =>
        (_repository, _localizer) = (repository, localizer);

    public async Task<Guid> Handle(UpdateStatusCounterRequest request, CancellationToken cancellationToken)
    {
        var counter = await _repository.GetByIdAsync(request.Id, cancellationToken);

        _ = counter ?? throw new NotFoundException(_localizer["counter.notfound"]);
        counter.UpdateStatus(request.IsActive);
        await _repository.UpdateAsync(counter, cancellationToken);
        return request.Id;
    }
}
