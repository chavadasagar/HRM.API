using HRM.API.Application.Catalog.Products;

namespace HRM.API.Application.Catalog.Brands;
public class UpdateStatusBrandRequest : IRequest<Guid>
{
    public Guid Id { get; set; }
    public bool IsActive { get; set; }
}

public class UpdateStatusBrandRequestHandler : IRequestHandler<UpdateStatusBrandRequest, Guid>
{
    private readonly IRepositoryWithEvents<Brand> _brandRepo;
    private readonly IReadRepository<Product> _productRepo;
    private readonly IStringLocalizer<UpdateStatusBrandRequestHandler> _localizer;

    public UpdateStatusBrandRequestHandler(IRepositoryWithEvents<Brand> brandRepo, IReadRepository<Product> productRepo, IStringLocalizer<UpdateStatusBrandRequestHandler> localizer) =>
        (_brandRepo, _productRepo, _localizer) = (brandRepo, productRepo, localizer);

    public async Task<Guid> Handle(UpdateStatusBrandRequest request, CancellationToken cancellationToken)
    {
        if (request.IsActive == false && (await _productRepo.AnyAsync(new ProductsByBrandSpec(request.Id), cancellationToken)))
        {
            throw new ConflictException(_localizer["brand.cannotbeinactive"]);
        }

        var brand = await _brandRepo.GetByIdAsync(request.Id, cancellationToken);

        _ = brand ?? throw new NotFoundException(_localizer["brand.notfound"]);
        brand.IsActive = request.IsActive;
        await _brandRepo.UpdateAsync(brand, cancellationToken);
        return request.Id;
    }
}
