using HRM.API.Domain.Identity;

namespace HRM.API.Application.Identity.Customers;

public class CreateCustomerRequest : IRequest<Guid>
{
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

public class CreateCustomerRequestValidator : CustomValidator<CreateCustomerRequest>
{
    public CreateCustomerRequestValidator(IReadRepository<Customer> repository, IStringLocalizer<CreateCustomerRequest> localizer)
    {
        RuleFor(p => p.Name)
                .NotEmpty()
                .MaximumLength(1024);

        When(_customer => !string.IsNullOrEmpty(_customer.Email), () =>
        {
            RuleFor(p => p.Email)
                 .MaximumLength(256)
                 .MustAsync(async (email, ct) => await repository.GetBySpecAsync(new CustomerByEmailSpec(email), ct) is null)
                     .WithMessage((_, email) => string.Format(localizer["customer.alreadyexists"], email));
        });

        When(_customer => !string.IsNullOrEmpty(_customer.Mobile), () =>
        {
            RuleFor(p => p.Mobile)
                .MaximumLength(16)
                .MustAsync(async (mobile, ct) => await repository.GetBySpecAsync(new CustomerByMobileSpec(mobile), ct) is null)
                    .WithMessage((_, mobile) => string.Format(localizer["customer.alreadyexists"], mobile));
        });

    }

}

public class CreateCustomerRequestHandler : IRequestHandler<CreateCustomerRequest, Guid>
{
    // Add Domain Events automatically by using IRepositoryWithEvents
    private readonly IRepositoryWithEvents<Customer> _repository;
    private readonly IStringLocalizer<CreateCustomerRequestHandler> _localizer;

    public CreateCustomerRequestHandler(IRepositoryWithEvents<Customer> repository, IStringLocalizer<CreateCustomerRequestHandler> localizer) =>
        (_repository, _localizer) = (repository, localizer);

    public async Task<Guid> Handle(CreateCustomerRequest request, CancellationToken cancellationToken)
    {
        var customer = new Customer(request.Name, request.Mobile, request.Email, request.Phone, request.GSTNumber, request.CountryId, request.StateId, request.City, request.Postcode, request.Address, request.IsActive);

        await _repository.AddAsync(customer, cancellationToken);

        return customer.Id;
    }
}