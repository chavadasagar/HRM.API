using MasterPOS.API.Domain.Inventory;

namespace MasterPOS.API.Application.Inventory;
public class CreateOrUpdatePurchaseRequest
{
    public Guid? Id { get; set; }
    public string? ReferenceNo { get; set; }
    public Guid StoreId { get; set; } = default!;
    public Guid SupplierId { get; set; } = default!;
    public string PurchaseDate { get; set; } = default!;
    public short PurchaseStatus { get; set; }
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
    public List<PurchaseProductRequest>? PurchaseProducts { get; set; }
    public PurchasePaymentRequest? PurchasePayments { get; set; }
}

public class CreateOrUpdatePurchaseRequestValidator : CustomValidator<CreateOrUpdatePurchaseRequest>
{
    public CreateOrUpdatePurchaseRequestValidator(IReadRepository<Store> storeRepo, IReadRepository<Supplier> supplierdRepo, IStringLocalizer<CreateOrUpdatePurchaseRequestValidator> localizer)
    {
        RuleFor(r => r.PurchaseDate)
            .NotEmpty();

        RuleFor(r => r.PurchaseProducts)
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
    }
}
