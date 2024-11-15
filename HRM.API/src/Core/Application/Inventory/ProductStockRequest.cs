using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MasterPOS.API.Application.Inventory;
public class ProductStockRequest
{
    public Guid ProductId { get; set; } = default!;
    public Guid StoreId { get; set; } = default!;
    public Guid? TypeId { get; set; }
}

public class ProductStockRequestValidator : CustomValidator<ProductStockRequest>
{
    public ProductStockRequestValidator(IReadRepository<Product> productRepo, IReadRepository<Store> storeRepo,  IStringLocalizer<CreateOrUpdatePurchaseRequestValidator> localizer)
    {
        RuleFor(p => p.ProductId)
               .NotEmpty()
               .MustAsync(async (id, ct) => await productRepo.GetByIdAsync(id, ct) is not null)
                   .WithMessage((_, id) => string.Format(localizer["product.notfound"], id));

        RuleFor(p => p.StoreId)
                .NotEmpty()
                .MustAsync(async (id, ct) => await storeRepo.GetByIdAsync(id, ct) is not null)
                    .WithMessage((_, id) => string.Format(localizer["store.notfound"], id));
    }
}