namespace HRM.API.Application.Common;
public class GetStatesRequest : IRequest<List<StateDto>>
{
    public Guid Id { get; set; }

    public GetStatesRequest(Guid id) => Id = id;
}

public class StatesRequestSpec : Specification<States, StateDto>
{
    public StatesRequestSpec(Guid countryId) =>
        Query.Where(p => p.CountryId == countryId);

}

public class GetStatesRequestHandler : IRequestHandler<GetStatesRequest, List<StateDto>>
{
    private readonly IReadRepository<States> _repository;

    public GetStatesRequestHandler(IReadRepository<States> repository) => _repository = repository;
    public async Task<List<StateDto>> Handle(GetStatesRequest request, CancellationToken cancellationToken)
    {
        var spec = new StatesRequestSpec(request.Id);
        return await _repository.ListAsync<StateDto>(spec, cancellationToken);
    }
}
