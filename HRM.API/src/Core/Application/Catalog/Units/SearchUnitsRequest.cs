using Unit = MasterPOS.API.Domain.Catalog.Unit;

namespace MasterPOS.API.Application.Catalog.Units;
public class SearchUnitsRequest : PaginationFilter, IRequest<PaginationResponse<UnitDto>>
{
}

public class UnitsBySearchRequestSpec : EntitiesByPaginationFilterSpec<Unit, UnitDto>
{
    public UnitsBySearchRequestSpec(SearchUnitsRequest request)
        : base(request) =>
        Query.OrderBy(c => c.Name, !request.HasOrderBy());
}

public class SearchUnitsRequestHandler : IRequestHandler<SearchUnitsRequest, PaginationResponse<UnitDto>>
{
    private readonly IReadRepository<Unit> _repository;

    public SearchUnitsRequestHandler(IReadRepository<Unit> repository) => _repository = repository;

    public async Task<PaginationResponse<UnitDto>> Handle(SearchUnitsRequest request, CancellationToken cancellationToken)
    {
        var spec = new UnitsBySearchRequestSpec(request);
        return await _repository.PaginatedListAsync(spec, request.PageNumber, request.PageSize, cancellationToken);
    }
}