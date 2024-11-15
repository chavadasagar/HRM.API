using Unit = HRM.API.Domain.Catalog.Unit;

namespace HRM.API.Application.Catalog.Units;
public class UpdateStatusUnitRequest : IRequest<Guid>
{
    public Guid Id { get; set; }
    public bool IsActive { get; set; }
}

public class UpdateStatusUnitRequestHandler : IRequestHandler<UpdateStatusUnitRequest, Guid>
{
    private readonly IRepositoryWithEvents<Unit> _repository;
    private readonly IReadRepository<Product> _productRepo;
    private readonly IStringLocalizer<UpdateStatusUnitRequestHandler> _localizer;

    public UpdateStatusUnitRequestHandler(IRepositoryWithEvents<Unit> repository, IReadRepository<Product> productRepo, IStringLocalizer<UpdateStatusUnitRequestHandler> localizer) =>
        (_repository, _productRepo, _localizer) = (repository, productRepo, localizer);

    public async Task<Guid> Handle(UpdateStatusUnitRequest request, CancellationToken cancellationToken)
    {
        if (request.IsActive == false && (await _productRepo.AnyAsync(new ProductsByUnitSpec(request.Id), cancellationToken)))
        {
            throw new ConflictException(_localizer["unit.cannotbeinactive"]);
        }

        var unit = await _repository.GetByIdAsync(request.Id, cancellationToken);

        _ = unit ?? throw new NotFoundException(_localizer["unit.notfound"]);
        unit.IsActive = request.IsActive;
        await _repository.UpdateAsync(unit, cancellationToken);
        return request.Id;
    }
}
