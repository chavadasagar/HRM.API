using HRM.API.Domain.Catalog;

namespace HRM.API.Domain.Inventory;
public class PurchaseProduct : AuditableEntity, IAggregateRoot
{
    public Guid PurchaseId { get; private set; } = default!;
    public Guid ProductId { get; private set; } = default!;
    public string? ProductName { get; private set; }
    public decimal Quantity { get; private set; }
    public decimal PurchasePrice { get; private set; }
    public decimal? Discount { get; private set; }
    public short? DiscountType { get; private set; }
    public decimal? TotalDiscount { get; private set; }
    public decimal CGST { get; private set; }
    public decimal SGST { get; private set; }
    public short TaxType { get; private set; }
    public decimal TotalTax { get; private set; }
    public decimal UnitCostAfterTaxAndDiscount { get; private set; }
    public decimal TotalAmount { get; private set; }
    public virtual Purchase Purchase { get; private set; } = default!;
    public virtual Product Product { get; private set; } = default!;

    public PurchaseProduct(Guid purchaseId, Guid productId, string? productName, decimal quantity, decimal purchasePrice, decimal? discount, short? discountType, decimal? totalDiscount, decimal cGST, decimal sGST, short taxType, decimal totalTax, decimal unitCostAfterTaxAndDiscount, decimal totalAmount)
    {
        PurchaseId = purchaseId;
        ProductId = productId;
        ProductName = productName;
        Quantity = quantity;
        PurchasePrice = purchasePrice;
        Discount = discount;
        DiscountType = discountType;
        TotalDiscount = totalDiscount;
        CGST = cGST;
        SGST = sGST;
        TaxType = taxType;
        TotalTax = totalTax;
        UnitCostAfterTaxAndDiscount = unitCostAfterTaxAndDiscount;
        TotalAmount = totalAmount;
    }

    public PurchaseProduct Update(Guid? productId, string? productName, decimal? quantity, decimal? purchasePrice, decimal? discount, short? discountType, decimal? totalDiscount, decimal? cGST, decimal? sGST, short? taxType, decimal? totalTax, decimal? unitCostAfterTaxAndDiscount, decimal? totalAmount)
    {
        if (productId.HasValue && productId.Value != Guid.Empty && !ProductId.Equals(productId.Value)) ProductId = productId.Value;
        if (productName is not null && ProductName?.Equals(productName) is not true) ProductName = productName;
        if (quantity.HasValue && Quantity != quantity) Quantity = quantity.Value;
        if (purchasePrice.HasValue && PurchasePrice != purchasePrice) PurchasePrice = purchasePrice.Value;
        Discount = null;
        if (discount.HasValue && Discount != discount) Discount = discount.Value;
        DiscountType = null;
        if (discountType.HasValue && DiscountType != discountType) DiscountType = discountType.Value;
        TotalDiscount = null;
        if (totalDiscount.HasValue && TotalDiscount != totalDiscount) TotalDiscount = totalDiscount.Value;
        if (taxType.HasValue && TaxType != taxType) TaxType = taxType.Value;
        if (sGST.HasValue && SGST != sGST) SGST = sGST.Value;
        if (cGST.HasValue && CGST != cGST) CGST = cGST.Value;
        if (totalTax.HasValue && TotalTax != totalTax) TotalTax = totalTax.Value;
        if (unitCostAfterTaxAndDiscount.HasValue && UnitCostAfterTaxAndDiscount != unitCostAfterTaxAndDiscount) UnitCostAfterTaxAndDiscount = unitCostAfterTaxAndDiscount.Value;
        if (totalAmount.HasValue && TotalAmount != totalAmount) TotalAmount = totalAmount.Value;
        return this;
    }
}
