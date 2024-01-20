namespace HRM.API.Domain.Catalog.Tickets;
public class Ticket : AuditableEntity, IAggregateRoot
{
    public string? Subject { get; set; }
    public int? TicketId { get; set; }
    public List<TicketStaff>? AssignStaff { get; set; }
    public DefaultIdType? ClientId { get; set; }
    public DefaultIdType? PriorityId { get; set; }
    public List<CCStaff>? CCStaff { get; set; }
    public Guid? AssigneeId { get; set; }
    public List<Followers>? Followers { get; set; }
    public string? Description { get; set; }
    public string? ImagePath { get; set; }
    public virtual Client? Client { get; set; }
    public virtual Priority? Priority { get; set; }
    public virtual Employee? Assignee { get; set; }

}