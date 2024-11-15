namespace MasterPOS.API.Application.Catalog.Counters;

public class UpdateCounterRequestValidator : CustomValidator<UpdateCounterRequest>
{
    public UpdateCounterRequestValidator(IReadRepository<Counter> CounterRepo, IReadRepository<Store> storeRepo, IStringLocalizer<UpdateCounterRequestValidator> localizer)
    {
        RuleFor(p => p.Name)
            .NotEmpty()
            .MaximumLength(256)
            .MustAsync(async (Counter, name, ct) =>
                    await CounterRepo.GetBySpecAsync(new CounterByNameSpec(name, Counter.StoreId), ct)
                        is not Counter existingCounter || existingCounter.Id == Counter.Id)
                .WithMessage((_, name) => string.Format(localizer["counter.alreadyexists"], name));

        RuleFor(p => p.StoreId)
            .NotEmpty()
            .MustAsync(async (id, ct) => await storeRepo.GetByIdAsync(id, ct) is not null)
                .WithMessage((_, id) => string.Format(localizer["storeRepo.notfound"], id));
    }
}