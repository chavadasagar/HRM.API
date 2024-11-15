using HRM.API.Domain.Inventory;

namespace HRM.API.Domain.Catalog;

public class Product : AuditableEntity, IAggregateRoot
{
    public string Name { get; private set; } = default!;
    public string Code { get; private set; } = default!;
    public long PCode { get; private set; } = default!;
    public Guid BrandId { get; private set; }
    public Guid CategoryId { get; private set; }
    public Guid UnitId { get; private set; }
    public string? SKU { get; private set; }
    public string? HSN { get; private set; }
    public long? AlertQuantity { get; private set; }
    public string? Barcode { get; private set; }
    public string? Description { get; private set; }
    public decimal BasePrice { get; private set; }
    public decimal CGST { get; private set; }
    public decimal SGST { get; private set; }
    public decimal PurchasePrice { get; private set; }
    public short TaxType { get; private set; }
    public decimal ProfitMargin { get; private set; }
    public decimal ProfitMarginAmount { get; private set; }
    public decimal SalesPrice { get; private set; }
    public string? ImagePath { get; private set; }
    public bool IsActive { get; private set; }
    public virtual Brand Brand { get; private set; } = default!;
    public virtual Category Category { get; private set; } = default!;
    public virtual Unit Unit { get; private set; } = default!;
    public List<PurchaseProduct>? PurchaseProducts { get; set; }
    public List<PurchaseReturnProduct>? PurchaseReturnProducts { get; set; }

    public Product(string name, long pCode, string code, Guid brandId, Guid categoryId, Guid unitId, string? sKU, string? hSN, long? alertQuantity, string? barcode, string? description, decimal basePrice, decimal cGST, decimal sGST, decimal purchasePrice, short taxType, decimal profitMargin, decimal profitMarginAmount, decimal salesPrice, string? imagePath, bool isActive)
    {
        this.Name = name;
        this.PCode = pCode;
        this.Code = code;
        this.BrandId = brandId;
        this.CategoryId = categoryId;
        this.UnitId = unitId;
        this.SKU = sKU;
        this.HSN = hSN;
        this.AlertQuantity = alertQuantity ?? null;
        this.Barcode = barcode;
        this.Description = description;
        this.BasePrice = basePrice;
        this.CGST = cGST;
        this.SGST = sGST;
        this.PurchasePrice = purchasePrice;
        this.TaxType = taxType;
        this.ProfitMargin = profitMargin;
        this.ProfitMarginAmount = profitMarginAmount;
        this.SalesPrice = salesPrice;
        this.ImagePath = imagePath;
        this.IsActive = isActive;
    }

    public Product Update(string? name, Guid? brandId, Guid? categoryId, Guid? unitId, string? sKU, string? hSN, long? alertQuantity, string? barcode, string? description, decimal? basePrice, decimal? cGST, decimal? sGST, decimal? purchasePrice, short? taxType, decimal? profitMargin, decimal? profitMarginAmount, decimal? salesPrice, string? imagePath, bool isActive)
    {
        if (name is not null && Name?.Equals(name) is not true) Name = name;
        if (brandId.HasValue && brandId.Value != Guid.Empty && !BrandId.Equals(brandId.Value)) BrandId = brandId.Value;
        if (categoryId.HasValue && categoryId.Value != Guid.Empty && !CategoryId.Equals(categoryId.Value)) CategoryId = categoryId.Value;
        if (unitId.HasValue && unitId.Value != Guid.Empty && !UnitId.Equals(unitId.Value)) UnitId = unitId.Value;
        if (sKU is not null && SKU?.Equals(sKU) is not true) SKU = sKU;
        if (hSN is not null && HSN?.Equals(hSN) is not true) HSN = hSN;
        AlertQuantity = null;
        if (alertQuantity.HasValue && AlertQuantity != alertQuantity) AlertQuantity = alertQuantity.Value;
        if (barcode is not null && Barcode?.Equals(barcode) is not true) Barcode = barcode;
        if (description is not null && Description?.Equals(description) is not true) Description = description;
        if (basePrice.HasValue && BasePrice != basePrice) BasePrice = basePrice.Value;
        if (cGST.HasValue && CGST != cGST) CGST = cGST.Value;
        if (sGST.HasValue && SGST != sGST) SGST = sGST.Value;
        if (purchasePrice.HasValue && PurchasePrice != purchasePrice) PurchasePrice = purchasePrice.Value;
        if (taxType.HasValue && TaxType != taxType) TaxType = taxType.Value;
        if (profitMargin.HasValue && ProfitMargin != profitMargin) ProfitMargin = profitMargin.Value;
        if (profitMarginAmount.HasValue && ProfitMarginAmount != profitMarginAmount) ProfitMarginAmount = profitMarginAmount.Value;
        if (salesPrice.HasValue && SalesPrice != salesPrice) SalesPrice = salesPrice.Value;
        if (imagePath is not null && ImagePath?.Equals(imagePath) is not true) ImagePath = imagePath;
        IsActive = isActive;
        return this;
    }

    public Product ClearImagePath()
    {
        ImagePath = string.Empty;
        return this;
    }

    public Product UpdateStatus(bool isActive)
    {
        IsActive = isActive;
        return this;
    }

}