namespace HRM.API.Application.Catalog.Products;

public class UpdateProductRequestValidator : CustomValidator<UpdateProductRequest>
{
    public UpdateProductRequestValidator(IReadRepository<Product> productRepo, IReadRepository<Brand> brandRepo, IReadRepository<Category> categoryRepo, IStringLocalizer<UpdateProductRequestValidator> localizer)
    {
        RuleFor(p => p.Name)
            .NotEmpty()
            .MaximumLength(75)
            .MustAsync(async (product, name, ct) =>
                    await productRepo.GetBySpecAsync(new ProductByNameSpec(name), ct)
                        is not Product existingProduct || existingProduct.Id == product.Id)
                .WithMessage((_, name) => string.Format(localizer["product.alreadyexists"], name));

        RuleFor(p => p.BrandId)
            .NotEmpty()
            .MustAsync(async (id, ct) => await brandRepo.GetByIdAsync(id, ct) is not null)
                .WithMessage((_, id) => string.Format(localizer["brand.notfound"], id));

        RuleFor(p => p.CategoryId)
            .NotEmpty()
            .MustAsync(async (id, ct) => await categoryRepo.GetByIdAsync(id, ct) is not null)
                .WithMessage((_, id) => string.Format(localizer["category.notfound"], id));

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