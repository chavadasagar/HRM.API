using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unit = HRM.API.Domain.Catalog.Unit;

namespace HRM.API.Application.Catalog.Units;
public class UpdateUnitRequest : IRequest<Guid>
{
    public Guid Id { get; set; }
    public string Name { get; set; } = default!;
    public string? Description { get; set; }
    public bool IsActive { get; set; }
}

public class UpdateUnitRequestValidator : CustomValidator<UpdateUnitRequest>
{
    public UpdateUnitRequestValidator(IRepository<Unit> repository, IStringLocalizer<UpdateUnitRequestValidator> localizer) =>
        RuleFor(p => p.Name)
            .NotEmpty()
            .MaximumLength(75)
            .MustAsync(async (unit, name, ct) =>
                    await repository.GetBySpecAsync(new UnitByNameSpec(name), ct)
                        is not Unit existingUnit || existingUnit.Id == unit.Id)
                .WithMessage((_, name) => string.Format(localizer["unit.alreadyexists"], name));
}

public class UpdateUnitRequestHandler : IRequestHandler<UpdateUnitRequest, Guid>
{
    // Add Domain Events automatically by using IRepositoryWithEvents
    private readonly IRepositoryWithEvents<Unit> _repository;
    private readonly IReadRepository<Product> _productRepo;
    private readonly IStringLocalizer<UpdateUnitRequestHandler> _localizer;

    public UpdateUnitRequestHandler(IRepositoryWithEvents<Unit> repository, IReadRepository<Product> productRepo, IStringLocalizer<UpdateUnitRequestHandler> localizer) =>
        (_repository, _productRepo, _localizer) = (repository, productRepo, localizer);

    public async Task<Guid> Handle(UpdateUnitRequest request, CancellationToken cancellationToken)
    {
        var unit = await _repository.GetByIdAsync(request.Id, cancellationToken);

        _ = unit ?? throw new NotFoundException(string.Format(_localizer["unit.notfound"], request.Id));

        if (request.IsActive == false && unit.IsActive == true && (await _productRepo.AnyAsync(new ProductsByUnitSpec(request.Id), cancellationToken)))
        {
            throw new ConflictException(_localizer["unit.cannotbeinactive"]);
        }

        unit.Update(request.Name, request.Description, request.IsActive);

        await _repository.UpdateAsync(unit, cancellationToken);

        return request.Id;
    }
}