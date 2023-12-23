using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRM.API.Application.Catalog.Products;
public class ProductByPurchaseReturnProductDto : IDto
{
    public Guid Id { get; set; } = default!;
    public Guid PurchaseReturnId { get; set; } = default!;
    public Guid ProductId { get; set; } = default!;
    public string? ProductName { get; set; }
    public decimal Quantity { get; set; }
    public string PurchaseReturnPurchaseReturnInvoiceId { get; set; } = default!;
    public DateTime PurchaseReturnPurchaseReturnDate { get; private set; } = default!;
}

public class ProductByPurchaseReturnDto : IDto
{
    public Guid Id { get; set; }
    public string? PurchaseReturnInvoiceId { get; set; }
    public string? ReferenceNo { get; set; }
    public string? PurchaseInvoiceId { get; set; }
}