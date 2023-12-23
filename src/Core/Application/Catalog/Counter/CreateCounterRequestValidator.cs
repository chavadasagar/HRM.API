namespace HRM.API.Application.Catalog.Counters;

public class CreateCounterRequestValidator : CustomValidator<CreateCounterRequest>
{
    public CreateCounterRequestValidator(IReadRepository<Counter> CounterRepo, IReadRepository<Store> storeRepo, IStringLocalizer<CreateCounterRequestValidator> localizer)
    {
        RuleFor(p => p.Name)
            .NotEmpty()
            .MaximumLength(75)
            .MustAsync(async (counter, name, ct) => await CounterRepo.GetBySpecAsync(new CounterByNameSpec(name, counter.StoreId), ct) is null)
                .WithMessage((_, name) => string.Format(localizer["counter.alreadyexists"], name));

        RuleFor(p => p.StoreId)
            .NotEmpty()
            .MustAsync(async (id, ct) => await storeRepo.GetByIdAsync(id, ct) is not null)
                .WithMessage((_, id) => string.Format(localizer["store.notfound"], id));
    }
}