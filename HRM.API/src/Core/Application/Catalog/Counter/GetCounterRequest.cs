namespace MasterPOS.API.Application.Catalog.Counters;

public class GetCounterRequest : IRequest<CounterDetailsDto>
{
    public Guid Id { get; set; }

    public GetCounterRequest(Guid id) => Id = id;
}

public class GetCounterRequestHandler : IRequestHandler<GetCounterRequest, CounterDetailsDto>
{
    private readonly IRepository<Counter> _repository;
    private readonly IStringLocalizer<GetCounterRequestHandler> _localizer;

    public GetCounterRequestHandler(IRepository<Counter> repository, IStringLocalizer<GetCounterRequestHandler> localizer) =>
        (_repository, _localizer) = (repository, localizer);

    public async Task<CounterDetailsDto> Handle(GetCounterRequest request, CancellationToken cancellationToken) =>
        await _repository.GetBySpecAsync(
            (ISpecification<Counter, CounterDetailsDto>)new CounterByIdWithStoreSpec(request.Id), cancellationToken)
        ?? throw new NotFoundException(string.Format(_localizer["counter.notfound"], request.Id));
}