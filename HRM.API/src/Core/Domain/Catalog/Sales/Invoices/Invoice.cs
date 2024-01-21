using HRM.API.Domain.Catalog.Sales.Estimates;
using HRM.API.Domain.Catalog.Sales.Taxs;

namespace HRM.API.Domain.Catalog.Sales.Invoices;
public class Invoice : AuditableEntity, IAggregateRoot
{
    public Guid? ClientId { get; set; }
    public Guid? ProjectId { get; set; }
    public string? Email { get; set; }
    public Guid? TaxId { get; set; }
    public string? ClientAddress { get; set; }
    public string? BillingAddress { get; set; }
    public DateTime? InvoiceDate { get; set; }
    public DateTime? DueDate { get; set; }
    public List<InvoiceItems>? InvoiceItems { get; set; }
    public string? OtherInformation { get; set; }
    public decimal? TotalAmount { get; set; }
    public decimal? DiscountPercentage { get; set; }
    public decimal? GrandTotal { get; set; }
    public virtual Client? Client { get; set; }
    public virtual Project? Project { get; set; }
    public virtual Tax? Tax { get; set; }
}
