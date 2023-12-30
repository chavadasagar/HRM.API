namespace HRM.API.Domain.Catalog;
public class TimeSheet : AuditableEntity, IAggregateRoot
{
    public TimeSheet(DefaultIdType? projectId, DateTime? deadline, decimal? totalHours, TimeSpan? remaining, DateTime? date, decimal? hours, string? description)
    {
        ProjectId = projectId;
        Deadline = deadline;
        TotalHours = totalHours;
        Remaining = remaining;
        Date = date;
        Hours = hours;
        Description = description;
    }

    public DefaultIdType? ProjectId { get; set; }
    public DateTime? Deadline { get; set; }
    public decimal? TotalHours { get; set; }
    public TimeSpan? Remaining { get; set; }
    public DateTime? Date { get; set; }
    public decimal? Hours { get; set; }
    public string? Description { get; set; }
    public virtual Project? Project { get; set; }
}
