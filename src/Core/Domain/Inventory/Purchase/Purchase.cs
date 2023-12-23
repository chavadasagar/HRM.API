using HRM.API.Domain.Catalog;

namespace HRM.API.Domain.Inventory;
public class Purchase : AuditableEntity, IAggregateRoot
{
    public long PIId { get; set; }
    public string PurchaseInvoiceId { get; private set; } = default!;
    public string? ReferenceNo { get; private set; }
    public Guid StoreId { get; private set; } = default!;
    public Guid SupplierId { get; private set; } = default!;
    public DateTime PurchaseDate { get; private set; } = default!;
    public short PurchaseStatus { get; private set; }
    public decimal TotalQuantity { get; private set; }
    public decimal CGST { get; private set; }
    public decimal SGST { get; private set; }
    public decimal TotalTax { get; private set; }
    public decimal? OtherCharge { get; private set; }
    public decimal? Discount { get; private set; }
    public short? DiscountType { get; private set; }
    public decimal? TotalDiscount { get; private set; }
    public decimal SubtotalAmount { get; private set; }
    public decimal TotalAmount { get; private set; }
    public string? Note { get; private set; }
    public virtual Store Store { get; private set; } = default!;
    public virtual Supplier Supplier { get; private set; } = default!;
    public List<PurchaseProduct>? PurchaseProducts { get; set; }
    public List<PurchasePayment>? PurchasePayments { get; set; }

    public Purchase(long pIId, string purchaseInvoiceId, string referenceNo, Guid storeId, Guid supplierId, DateTime purchaseDate, short purchaseStatus, decimal totalQuantity, decimal cGST, decimal sGST, decimal totalTax, decimal? otherCharge, decimal? discount, short? discountType, decimal? totalDiscount, decimal subtotalAmount, decimal totalAmount, string? note)
    {
        PIId = pIId;
        PurchaseInvoiceId = purchaseInvoiceId;
        ReferenceNo = referenceNo;
        StoreId = storeId;
        SupplierId = supplierId;
        PurchaseDate = purchaseDate;
        PurchaseStatus = purchaseStatus;
        TotalQuantity = totalQuantity;
        CGST = cGST;
        SGST = sGST;
        TotalTax = totalTax;
        OtherCharge = otherCharge;
        Discount = discount;
        DiscountType = discountType;
        TotalDiscount = totalDiscount;
        SubtotalAmount = subtotalAmount;
        TotalAmount = totalAmount;
        Note = note;
    }

    public Purchase Update(string? referenceNo, Guid? storeId, Guid? supplierId, DateTime? purchaseDate, short? purchaseStatus, decimal? totalQuantity, decimal? cGST, decimal? sGST, decimal? totalTax, decimal? otherCharge, decimal? discount, short? discountType, decimal? totalDiscount, decimal? subtotalAmount, decimal? totalAmount, string? note)
    {
        if (referenceNo is not null && ReferenceNo?.Equals(referenceNo) is not true) ReferenceNo = referenceNo;
        if (storeId.HasValue && storeId.Value != Guid.Empty && !StoreId.Equals(storeId.Value)) StoreId = storeId.Value;
        if (supplierId.HasValue && supplierId.Value != Guid.Empty && !SupplierId.Equals(supplierId.Value)) SupplierId = supplierId.Value;
        if (purchaseDate is not null) PurchaseDate = purchaseDate.Value;
        if (purchaseStatus.HasValue && PurchaseStatus != purchaseStatus) PurchaseStatus = purchaseStatus.Value;
        if (totalQuantity.HasValue && TotalQuantity != totalQuantity) TotalQuantity = totalQuantity.Value;
        if (sGST.HasValue && SGST != sGST) SGST = sGST.Value;
        if (cGST.HasValue && CGST != cGST) CGST = cGST.Value;
        if (totalTax.HasValue && TotalTax != totalTax) TotalTax = totalTax.Value;
        OtherCharge = null;
        if (otherCharge.HasValue && OtherCharge != otherCharge) OtherCharge = otherCharge.Value;
        Discount = null;
        if (discount.HasValue && Discount != discount) Discount = discount.Value;
        DiscountType = null;
        if (discountType.HasValue && DiscountType != discountType) DiscountType = discountType.Value;
        TotalDiscount = null;
        if (totalDiscount.HasValue && TotalDiscount != totalDiscount) TotalDiscount = totalDiscount.Value;
        if (subtotalAmount.HasValue && SubtotalAmount != subtotalAmount) SubtotalAmount = subtotalAmount.Value;
        if (totalAmount.HasValue && TotalAmount != totalAmount) TotalAmount = totalAmount.Value;
        Note = note;
        return this;
    }

}
