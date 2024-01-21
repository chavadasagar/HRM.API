namespace HRM.API.Domain.Catalog.Sales;
public class PaymentType : AuditableEntity, IAggregateRoot
{
    public PaymentType(string? name, string? description)
    {
        Name = name;
        Description = description;
    }

    public string? Name { get; set; } = "Cash";
    public string? Description { get; set; }
}
