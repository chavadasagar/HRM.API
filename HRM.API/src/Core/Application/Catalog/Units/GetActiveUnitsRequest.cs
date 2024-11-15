using Unit = MasterPOS.API.Domain.Catalog.Unit;

namespace MasterPOS.API.Application.Catalog;

public class GetActiveUnitsRequest : IRequest<List<UnitDropdownDto>>
{
}

public class UnitsDropdownRequestSpec : Specification<Unit, UnitDropdownDto>
{
    public UnitsDropdownRequestSpec()
    {
        Query
            .Where(c => c.IsActive)
            .OrderBy(c => c.Name);
    }
}

public class GetActiveUnitsRequestHandler : IRequestHandler<GetActiveUnitsRequest, List<UnitDropdownDto>>
{
    private readonly IReadRepository<Unit> _repository;

    public GetActiveUnitsRequestHandler(IReadRepository<Unit> repository) => _repository = repository;
    public async Task<List<UnitDropdownDto>> Handle(GetActiveUnitsRequest request, CancellationToken cancellationToken)
    {
        var spec = new UnitsDropdownRequestSpec();
        return await _repository.ListAsync<UnitDropdownDto>(spec, cancellationToken);
    }
}