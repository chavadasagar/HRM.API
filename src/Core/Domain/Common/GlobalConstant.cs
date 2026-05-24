namespace HRM.API.Domain.Common;
public static class GlobalConstant
{

    #region  Directory List
    public static readonly string CategoryImageUploadDirectory = @"Files\Category\";
    public static readonly string BrandImageUploadDirectory = @"Files\brand\";
    public static readonly string PaymentTypeImageUploadDirectory = @"Files\paymenttype\";
    public static readonly string StoreImageUploadDirectory = @"Files\store\";
    public static readonly string CompanyImageUploadDirectory = @"Files\company\";
    public static readonly string UserImageUploadDirectory = @"Files\ApplicationUser\";
    public static readonly string ProductImageUploadDirectory = @"Files\Product\";
    #endregion

    #region  Prefex
    public static readonly string PrefixStore = "ST";
    public static readonly string PrefixProduct = "P";
    public static readonly string PrefixPurchaseInvoice = "PII";
    public static readonly string PrefixPurchaseReturnInvoice = "PRII";
    #endregion

}
