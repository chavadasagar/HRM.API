using HRM.API.Domain.Inventory;

namespace HRM.API.Application.Inventory;
public class CreateOrUpdatePurchaseReturnRequest
{
    public Guid? Id { get; set; }
    public string? PurchaseInvoiceId { get; set; }
    public string? ReferenceNo { get; set; }
    public Guid StoreId { get; set; } = default!;
    public Guid SupplierId { get; set; } = default!;
    public string PurchaseReturnDate { get; set; } = default!;
    public short PurchaseReturnStatus { get; set; }
    public decimal TotalQuantity { get; set; }
    public decimal CGST { get; set; }
    public decimal SGST { get; set; }
    public decimal TotalTax { get; set; }
    public decimal? OtherCharge { get; set; }
    public decimal? Discount { get; set; }
    public short? DiscountType { get; set; }
    public decimal? TotalDiscount { get; set; }
    public decimal SubtotalAmount { get; set; }
    public decimal TotalAmount { get; set; }
    public string? Note { get; set; }
    public List<PurchaseReturnProductRequest>? PurchaseReturnProducts { get; set; }
    public PurchaseReturnPaymentRequest? PurchaseReturnPayments { get; set; }
}

public class CreateOrUpdatePurchaseReturnRequestValidator : CustomValidator<CreateOrUpdatePurchaseReturnRequest>
{
    public CreateOrUpdatePurchaseReturnRequestValidator(IInventoryService inventoryService, IReadRepository<Store> storeRepo, IReadRepository<Supplier> supplierdRepo, IStringLocalizer<CreateOrUpdatePurchaseReturnRequestValidator> localizer)
    {
        RuleFor(r => r.PurchaseReturnDate)
            .NotEmpty();

        RuleFor(r => r.PurchaseReturnProducts)
            .NotNull();

        RuleFor(p => p.StoreId)
                .NotEmpty()
                .MustAsync(async (id, ct) => await storeRepo.GetByIdAsync(id, ct) is not null)
                    .WithMessage((_, id) => string.Format(localizer["store.notfound"], id));

        RuleFor(p => p.SupplierId)
                .NotEmpty()
                .MustAsync(async (id, ct) => await supplierdRepo.GetByIdAsync(id, ct) is not null)
                    .WithMessage((_, id) => string.Format(localizer["supplier.notfound"], id));

        RuleFor(p => p.TotalQuantity)
            .GreaterThanOrEqualTo(0);

        RuleFor(p => p.SubtotalAmount)
            .GreaterThanOrEqualTo(0);

        RuleFor(p => p.TotalAmount)
          .GreaterThanOrEqualTo(0);

        RuleForEach(x => x.PurchaseReturnProducts)
            .SetValidator((x, ct) => new PurchaseReturnProductRequestValidator(localizer, inventoryService));
    }
}

public class PurchaseReturnProductRequestValidator : CustomValidator<PurchaseReturnProductRequest>
{
    public PurchaseReturnProductRequestValidator(IStringLocalizer<CreateOrUpdatePurchaseReturnRequestValidator> localizer, IInventoryService inventoryService)
    {
        RuleFor(p => p.StoreId)
               .NotEmpty();

        RuleFor(p => p)
               .NotEmpty()
               .MustAsync(async (ct, id) => await inventoryService.GetProductStock(ct.ProductId, ct.StoreId, ct.PurchaseReturnId ?? Guid.Empty) >= ct.Quantity)
               .WithMessage((ct, id) => string.Format(localizer["purchasereturn.quantitynotavailable"], ct.ProductName, ct.Quantity));
    }
}