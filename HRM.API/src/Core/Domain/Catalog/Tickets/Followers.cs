namespace HRM.API.Domain.Catalog.Tickets;
public class Followers : AuditableEntity, IAggregateRoot
{
    public DefaultIdType? TicketId { get; set; }
    public DefaultIdType? EmployeeId { get; set; }
    public virtual Employee? Employee { get; set; }
    public virtual Ticket? Ticket { get; set; }
}