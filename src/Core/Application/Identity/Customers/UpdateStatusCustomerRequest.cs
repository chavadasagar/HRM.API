using HRM.API.Domain.Identity;

namespace HRM.API.Application.Identity.Customers;
public class UpdateStatusCustomerRequest : IRequest<Guid>
{
    public Guid Id { get; set; }
    public bool IsActive { get; set; }
}

public class UpdateStatusCustomerRequestHandler : IRequestHandler<UpdateStatusCustomerRequest, Guid>
{
    private readonly IRepositoryWithEvents<Customer> _repository;
    private readonly IStringLocalizer<UpdateStatusCustomerRequestHandler> _localizer;

    public UpdateStatusCustomerRequestHandler(IRepositoryWithEvents<Customer> repository, IStringLocalizer<UpdateStatusCustomerRequestHandler> localizer) =>
        (_repository, _localizer) = (repository, localizer);

    public async Task<Guid> Handle(UpdateStatusCustomerRequest request, CancellationToken cancellationToken)
    {
        var store = await _repository.GetByIdAsync(request.Id, cancellationToken);

        _ = store ?? throw new NotFoundException(_localizer["customer.notfound"]);
        store.UpdateStatus(request.IsActive);
        await _repository.UpdateAsync(store, cancellationToken);
        return request.Id;
    }
}
