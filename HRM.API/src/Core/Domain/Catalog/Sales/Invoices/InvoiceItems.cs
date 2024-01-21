using HRM.API.Domain.Catalog.Sales.Invoices;

namespace HRM.API.Domain.Catalog.Sales.Estimates;
public class InvoiceItems : AuditableEntity, IAggregateRoot
{
    public DefaultIdType? InvoiceId { get; set; }
    public Guid? ItemId { get; set; }
    public string? Description { get; set; }
    public decimal? UnitCost { get; set; }
    public int? Quantity { get; set; }
    public decimal? Amount { get; set; }
    public virtual Invoice? Invoice { get; set; }
    public virtual Item? Item { get; set; }
}
