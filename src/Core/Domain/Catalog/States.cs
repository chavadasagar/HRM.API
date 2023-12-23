namespace HRM.API.Domain.Catalog;
public class States : AuditableEntity, IAggregateRoot
{
    public States()
    {
    }

    public States(string name, string? latitude, string? longitude)
    {
        Name = name;
        this.Latitude = latitude;
        this.Longitude = longitude;
    }

    public States(string name, string? latitude, string? longitude, List<Cities>? cities)
    {
        Name = name;
        this.Latitude = latitude;
        this.Longitude = longitude;
        Cities = cities;
    }

    public string Name { get; set; }
    public string? Latitude { get; set; }
    public string? Longitude { get; set; }
    public Guid? CountryId { get; set; }
    public List<Cities>? Cities { get; set; }
}