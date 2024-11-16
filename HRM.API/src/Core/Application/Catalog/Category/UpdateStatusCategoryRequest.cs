namespace HRM.API.Application.Catalog;

public class UpdateStatusCategoryRequest : IRequest<Guid>
{
    public Guid Id { get; set; }
    public bool IsActive { get; set; }
}

public class UpdateStatusCategoryRequestHandler : IRequestHandler<UpdateStatusCategoryRequest, Guid>
{
    private readonly IRepositoryWithEvents<Category> _repository;
    private readonly IStringLocalizer<UpdateStatusCategoryRequestHandler> _localizer;

    public UpdateStatusCategoryRequestHandler(IRepositoryWithEvents<Category> repository,IStringLocalizer<UpdateStatusCategoryRequestHandler> localizer) =>
        (_repository, _localizer) = (repository, localizer);

    public async Task<Guid> Handle(UpdateStatusCategoryRequest request, CancellationToken cancellationToken)
    {
        var category = await _repository.GetByIdAsync(request.Id, cancellationToken);

        _ = category ?? throw new NotFoundException(_localizer["category.notfound"]);

        category.IsActive = request.IsActive;

        await _repository.UpdateAsync(category, cancellationToken);

        return request.Id;
    }
}

