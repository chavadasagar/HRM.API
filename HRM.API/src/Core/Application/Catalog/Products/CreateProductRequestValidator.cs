using Unit = MasterPOS.API.Domain.Catalog.Unit;

namespace MasterPOS.API.Application.Catalog.Products;

public class CreateProductRequestValidator : CustomValidator<CreateProductRequest>
{
    public CreateProductRequestValidator(IReadRepository<Product> productRepo, IReadRepository<Brand> brandRepo, IReadRepository<Category> categoryRepo, IReadRepository<Unit> unitRepo, IStringLocalizer<CreateProductRequestValidator> localizer)
    {
        RuleFor(p => p.Name)
            .NotEmpty()
            .MaximumLength(75)
            .MustAsync(async (name, ct) => await productRepo.GetBySpecAsync(new ProductByNameSpec(name), ct) is null)
                .WithMessage((_, name) => string.Format(localizer["product.alreadyexists"], name));

        RuleFor(p => p.BrandId)
            .NotEmpty()
            .MustAsync(async (id, ct) => await brandRepo.GetByIdAsync(id, ct) is not null)
                .WithMessage((_, id) => string.Format(localizer["brand.notfound"], id));

        RuleFor(p => p.CategoryId)
            .NotEmpty()
            .MustAsync(async (id, ct) => await categoryRepo.GetByIdAsync(id, ct) is not null)
                .WithMessage((_, id) => string.Format(localizer["category.notfound"], id));

        RuleFor(p => p.UnitId)
            .NotEmpty()
            .MustAsync(async (id, ct) => await unitRepo.GetByIdAsync(id, ct) is not null)
                .WithMessage((_, id) => string.Format(localizer["unit.notfound"], id));

        //RuleFor(p => p.BasePrice)
        //    .GreaterThanOrEqualTo(1);

        //RuleFor(p => p.PurchasePrice)
        //    .GreaterThanOrEqualTo(1);

        //RuleFor(p => p.SalesPrice)
        //  .GreaterThanOrEqualTo(1);

        //RuleFor(p => p.Image)
        //    .SetNonNullableValidator(new FileUploadRequestValidator());

    }
}