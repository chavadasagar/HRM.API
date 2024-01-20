namespace HRM.API.Domain.Catalog.Estimates;
public class EstimateItems : AuditableEntity, IAggregateRoot
{
    public Guid? EstimateId { get; set; }
    public string? ItemName { get; set; }
    public string? Description { get; set; }
    public decimal? UnitCost { get; set; }
    public int? Quantity { get; set; }
    public Designation? Amount { get; set; }
    public Estimate? Estimate { get; set; }
}
