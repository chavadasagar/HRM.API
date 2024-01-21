namespace HRM.API.Domain.Catalog.Sales.Estimates;
public class EstimateItems : AuditableEntity, IAggregateRoot
{
    public Guid? EstimateId { get; set; }
    public Guid? ItemId { get; set; }
    public string? Description { get; set; }
    public decimal? UnitCost { get; set; }
    public int? Quantity { get; set; }
    public decimal? Amount { get; set; }
    public virtual Estimate? Estimate { get; set; }
    public virtual Item? Item { get; set; }
}
