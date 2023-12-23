namespace HRM.API.Domain.Catalog;
public class Cities : AuditableEntity, IAggregateRoot
{
    public Cities()
    {

    }
    public Cities(string name, string? latitude, string? longitude)
    {
        Name = name;
        this.Latitude = latitude;
        this.Longitude = longitude;
    }

    public string Name { get; set; }
    public string? Latitude { get; set; }
    public string? Longitude { get; set; }
    public Guid? StatesId { get; set; }
}

