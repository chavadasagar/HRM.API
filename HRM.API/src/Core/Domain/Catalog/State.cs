namespace HRM.API.Domain.Catalog;
public class State : AuditableEntity, IAggregateRoot
{
    public State()
    {
    }

    public State(string name, string? latitude, string? longitude)
    {
        Name = name;
        this.Latitude = latitude;
        this.Longitude = longitude;
    }

    public State(string name, string? latitude, string? longitude, List<City>? cities)
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
    public List<City>? Cities { get; set; }
}