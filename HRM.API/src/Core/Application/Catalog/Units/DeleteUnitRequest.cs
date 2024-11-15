using Unit = HRM.API.Domain.Catalog.Unit;

namespace HRM.API.Application.Catalog.Units;

public class DeleteUnitRequest : IRequest<string>
{
    public Guid Id { get; set; }
    public DeleteUnitRequest(Guid id) => Id = id;
}

public class DeleteBrandRequestHandler : IRequestHandler<DeleteUnitRequest, string>
{
    private readonly IRepositoryWithEvents<Unit> _repository;
    private readonly IReadRepository<Product> _productRepo;
    private readonly IStringLocalizer<DeleteBrandRequestHandler> _localizer;

    public DeleteBrandRequestHandler(IRepositoryWithEvents<Unit> repository, IReadRepository<Product> productRepo, IStringLocalizer<DeleteBrandRequestHandler> localizer) =>
        (_repository,  _localizer, _productRepo) = (repository,  localizer, productRepo);

    public async Task<string> Handle(DeleteUnitRequest request, CancellationToken cancellationToken)
    {
        if (await _productRepo.AnyAsync(new ProductsByUnitSpec(request.Id), cancellationToken))
        {
            throw new ConflictException(_localizer["unit.cannotbedeleted"]);
        }

        var unit = await _repository.GetByIdAsync(request.Id, cancellationToken);

        _ = unit ?? throw new NotFoundException(_localizer["unit.notfound"]);

        await _repository.DeleteAsync(unit, cancellationToken);

        return unit.Name;
    }
}
