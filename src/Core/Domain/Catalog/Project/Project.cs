namespace HRM.API.Domain.Catalog;
public class Project : AuditableEntity, IAggregateRoot
{
    public Project(DefaultIdType? clientId, DateTime? startDate, DateTime? endDate, decimal? rate, DefaultIdType? rateTypeId, DefaultIdType? priorityId, string? description, string? filepath)
    {
        ClientId = clientId;
        StartDate = startDate;
        EndDate = endDate;
        Rate = rate;
        RateTypeId = rateTypeId;
        PriorityId = priorityId;
        Description = description;
        Filepath = filepath;
    }

    public DefaultIdType? ClientId { get; set; }
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public decimal? Rate { get; set; }
    public DefaultIdType? RateTypeId { get; set; }
    public DefaultIdType? PriorityId { get; set; }
    public List<ProjectTeamLeaders>? ProjectTeamLeaders { get; set; }
    public List<ProjectTeam>? ProjectTeam { get; set; }
    public string? Description { get; set; }
    public string? Filepath { get; set; }
    public virtual Client? Client { get; set; }
    public virtual RateType? RateType { get; set; }
    public virtual Priority? Priority { get; set; }
}
