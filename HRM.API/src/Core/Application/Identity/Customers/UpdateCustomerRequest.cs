using MasterPOS.API.Domain.Identity;

namespace MasterPOS.API.Application.Identity.Customers;

public class UpdateCustomerRequest : IRequest<Guid>
{
    public Guid Id { get; set; }
    public string Name { get; set; } = default!;
    public string? Mobile { get; set; }
    public string? Email { get; set; }
    public string? Phone { get; set; }
    public string? GSTNumber { get; set; }
    public Guid? CountryId { get; set; }
    public Guid? StateId { get; set; }
    public string? City { get; set; }
    public string? Postcode { get; set; }
    public string? Address { get; set; }
    public bool IsActive { get; set; }
}

public class UpdateCustomerRequestValidator : CustomValidator<UpdateCustomerRequest>
{
    public UpdateCustomerRequestValidator(IRepository<Customer> repository, IStringLocalizer<UpdateCustomerRequestValidator> localizer)
    {
        RuleFor(p => p.Name)
           .NotEmpty()
           .MaximumLength(1024);

        When(_customer => !string.IsNullOrEmpty(_customer.Email), () =>
        {
            RuleFor(p => p.Email)
        .MaximumLength(256)
        .MustAsync(async (customer, email, ct) =>
                await repository.GetBySpecAsync(new CustomerByEmailSpec(email), ct)
                    is not Customer existingCustomer || existingCustomer.Id == customer.Id)
            .WithMessage((_, email) => string.Format(localizer["customer.alreadyexists"], email));
        });

        When(_customer => !string.IsNullOrEmpty(_customer.Mobile), () =>
        {
            RuleFor(p => p.Mobile)
              .MaximumLength(16)
              .MustAsync(async (customer, mobile, ct) =>
                      await repository.GetBySpecAsync(new CustomerByMobileSpec(mobile), ct)
                          is not Customer existingCustomer || existingCustomer.Id == customer.Id)
                  .WithMessage((_, mobile) => string.Format(localizer["customer.alreadyexists"], mobile));
        });
    }
}

public class UpdateCustomerRequestHandler : IRequestHandler<UpdateCustomerRequest, Guid>
{
    // Add Domain Events automatically by using IRepositoryWithEvents
    private readonly IRepositoryWithEvents<Customer> _repository;
    private readonly IStringLocalizer<UpdateCustomerRequestHandler> _localizer;

    public UpdateCustomerRequestHandler(IRepositoryWithEvents<Customer> repository, IStringLocalizer<UpdateCustomerRequestHandler> localizer) =>
        (_repository, _localizer) = (repository, localizer);

    public async Task<Guid> Handle(UpdateCustomerRequest request, CancellationToken cancellationToken)
    {
        var customer = await _repository.GetByIdAsync(request.Id, cancellationToken);

        _ = customer ?? throw new NotFoundException(string.Format(_localizer["customer.notfound"], request.Id));

        customer.Update(request.Name, request.Mobile, request.Email, request.Phone, request.GSTNumber, request.CountryId, request.StateId, request.City, request.Postcode, request.Address, request.IsActive);

        await _repository.UpdateAsync(customer, cancellationToken);

        return request.Id;
    }
}