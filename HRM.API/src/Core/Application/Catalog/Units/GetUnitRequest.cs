using Unit = MasterPOS.API.Domain.Catalog.Unit;

namespace MasterPOS.API.Application.Catalog.Units;
public class GetUnitRequest : IRequest<UnitDto>
{
    public Guid Id { get; set; }

    public GetUnitRequest(Guid id) => Id = id;
}

public class UnitByIdSpec : Specification<Unit, UnitDto>, ISingleResultSpecification
{
    public UnitByIdSpec(Guid id) =>
        Query.Where(p => p.Id == id);
}

public class GetUnitRequestHandler : IRequestHandler<GetUnitRequest, UnitDto>
{
    private readonly IRepository<Unit> _repository;
    private readonly IStringLocalizer<GetUnitRequestHandler> _localizer;

    public GetUnitRequestHandler(IRepository<Unit> repository, IStringLocalizer<GetUnitRequestHandler> localizer) => (_repository, _localizer) = (repository, localizer);

    public async Task<UnitDto> Handle(GetUnitRequest request, CancellationToken cancellationToken) =>
        await _repository.GetBySpecAsync(
            (ISpecification<Unit, UnitDto>)new UnitByIdSpec(request.Id), cancellationToken)
        ?? throw new NotFoundException(string.Format(_localizer["unit.notfound"], request.Id));
}