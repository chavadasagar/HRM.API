namespace HRM.API.Domain.Catalog.Sales.ProvidentFunds;
public class ProvidentFund : AuditableEntity, IAggregateRoot
{
    public Guid? EmployeeId { get; set; }
    public Guid? FundTypeId { get; set; }
    public decimal? EmployeeShareInAmount { get; set; }
    public decimal? OrganizationShareInAmount { get; set; }
    public decimal? EmployeeShareInPercentage { get; set; }
    public decimal? OrganizationShareInPercentage { get; set; }
    public string? Description { get; set; }
    public virtual Employee? Employee { get; set; }
    public virtual FundType? FundType { get; set; }
}
