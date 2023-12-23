namespace HRM.API.Application.Catalog.Products;
public class UpdateStatusProductRequest : IRequest<Guid>
{
    public Guid Id { get; set; }
    public bool IsActive { get; set; }
}

public class UpdateStatusProductRequestHandler : IRequestHandler<UpdateStatusProductRequest, Guid>
{
    private readonly IRepositoryWithEvents<Product> _repository;
    private readonly IStringLocalizer<UpdateStatusProductRequestHandler> _localizer;

    public UpdateStatusProductRequestHandler(IRepositoryWithEvents<Product> repository, IStringLocalizer<UpdateStatusProductRequestHandler> localizer) =>
        (_repository, _localizer) = (repository, localizer);

    public async Task<Guid> Handle(UpdateStatusProductRequest request, CancellationToken cancellationToken)
    {

        var product = await _repository.GetByIdAsync(request.Id, cancellationToken);

        _ = product ?? throw new NotFoundException(_localizer["product.notfound"]);
        product.UpdateStatus(request.IsActive);
        await _repository.UpdateAsync(product, cancellationToken);
        return request.Id;
    }
}


