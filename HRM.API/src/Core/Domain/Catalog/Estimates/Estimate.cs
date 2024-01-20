namespace HRM.API.Domain.Catalog.Estimates;
public class Estimate : AuditableEntity, IAggregateRoot
{
    public Guid? ClientId { get; set; }
    public Guid? ProjectId { get; set; }
    public string? Email { get; set; }
    public Guid? TaxId { get; set; }
    public string? ClientAddress { get; set; }
    public string? BillingAddress { get; set; }
    public DateTime? EstimateDate { get; set; }
    public DateTime? ExpiryDate { get; set; }
    public List<EstimateItems>? EstimateItems { get; set; }
    public string? OtherInformation { get; set; }
    public decimal? TotalAmount { get; set; }
    public decimal? DiscountPercentage { get; set; }
    public decimal? GrandTotal { get; set; }
    public virtual Client? Client { get; set; }
    public virtual Project? Project { get; set; }
    public virtual Tax? Tax { get; set; }
}
