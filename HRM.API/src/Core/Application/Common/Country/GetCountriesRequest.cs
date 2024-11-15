namespace MasterPOS.API.Application.Common;
public class GetCountriesRequest : IRequest<List<CountryDto>>
{
}

public class CountriesRequestSpec : Specification<Country, CountryDto>
{
    public CountriesRequestSpec(GetCountriesRequest request)
    {
        Query.OrderBy(c => c.Name);
    }
}

public class GetCountriesRequestHandler : IRequestHandler<GetCountriesRequest, List<CountryDto>>
{
    private readonly IReadRepository<Country> _repository;

    public GetCountriesRequestHandler(IReadRepository<Country> repository) => _repository = repository;
    public async Task<List<CountryDto>> Handle(GetCountriesRequest request, CancellationToken cancellationToken)
    {
        var spec = new CountriesRequestSpec(request);
        return await _repository.ListAsync<CountryDto>(spec, cancellationToken);
    }
}