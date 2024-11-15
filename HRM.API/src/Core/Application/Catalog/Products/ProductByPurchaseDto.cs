using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MasterPOS.API.Application.Catalog.Products;
public class ProductByPurchaseProductDto : IDto
{
    public Guid Id { get; set; } = default!;
    public Guid PurchaseId { get; set; } = default!;
    public Guid ProductId { get; set; } = default!;
    public string? ProductName { get; set; }
    public decimal Quantity { get; set; }
    public string PurchasePurchaseInvoiceId { get; set; } = default!;
    public DateTime PurchasePurchaseDate { get; set; } = default!;
}

public class ProductByPurchaseDto : IDto
{
    public Guid Id { get; set; }
    public string? PurchaseInvoiceId { get; set; }
    public string? ReferenceNo { get; set; }
}
