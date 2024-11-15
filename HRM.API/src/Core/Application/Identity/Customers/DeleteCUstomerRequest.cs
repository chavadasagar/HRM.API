using MasterPOS.API.Application.Catalog.Counters;
using MasterPOS.API.Domain.Identity;

namespace MasterPOS.API.Application.Identity.Customers;
public class DeleteCustomerRequest : IRequest<string>
{
    public Guid Id { get; set; }

    public DeleteCustomerRequest(Guid id) => Id = id;
}

public class DeleteCustomerRequestHandler : IRequestHandler<DeleteCustomerRequest, string>
{
    // Add Domain Events automatically by using IRepositoryWithEvents
    private readonly IRepositoryWithEvents<Customer> _repository;
    private readonly IStringLocalizer<DeleteCustomerRequestHandler> _localizer;
    private readonly IFileStorageService _file;

    public DeleteCustomerRequestHandler(IRepositoryWithEvents<Customer> repository, IStringLocalizer<DeleteCustomerRequestHandler> localizer) =>
        (_repository, _localizer) = (repository, localizer);

    public async Task<string> Handle(DeleteCustomerRequest request, CancellationToken cancellationToken)
    {
        var customer = await _repository.GetByIdAsync(request.Id, cancellationToken);

        _ = customer ?? throw new NotFoundException(_localizer["customer.notfound"]);

        await _repository.DeleteAsync(customer, cancellationToken);

        return customer.Name;
    }
}