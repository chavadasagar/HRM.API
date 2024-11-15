using Unit = MasterPOS.API.Domain.Catalog.Unit;

namespace MasterPOS.API.Application.Catalog.Units;
public class CreateUnitRequest : IRequest<Guid>
{
    public string Name { get; set; } = default!;
    public string? Description { get; set; }
    public bool IsActive { get; set; }
}

public class CreateUnitRequestValidator : CustomValidator<CreateUnitRequest>
{
    public CreateUnitRequestValidator(IReadRepository<Unit> repository, IStringLocalizer<CreateUnitRequestValidator> localizer) =>
        RuleFor(p => p.Name)
            .NotEmpty()
            .MaximumLength(75)
            .MustAsync(async (name, ct) => await repository.GetBySpecAsync(new UnitByNameSpec(name), ct) is null)
                .WithMessage((_, name) => string.Format(localizer["unit.alreadyexists"], name));
}

public class CreateUnitRequestHandler : IRequestHandler<CreateUnitRequest, Guid>
{
    // Add Domain Events automatically by using IRepositoryWithEvents
    private readonly IRepositoryWithEvents<Unit> _repository;

    public CreateUnitRequestHandler(IRepositoryWithEvents<Unit> repository) => _repository = repository;

    public async Task<Guid> Handle(CreateUnitRequest request, CancellationToken cancellationToken)
    {
        var unit = new Unit(request.Name, request.Description, request.IsActive);

        await _repository.AddAsync(unit, cancellationToken);

        return unit.Id;
    }
}