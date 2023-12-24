namespace HRM.API.Domain.Catalog;
public class Holidays : AuditableEntity, IAggregateRoot
{
    public string? Name { get; set; }
    public DateTime? Date { get; set; }
    public bool? IsActive { get; set; }
}
