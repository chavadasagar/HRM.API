namespace HRM.API.Application.Common;
public class CityDto : IDto
{
    public Guid? Id { get; set; }
    public string Name { get; set; }
    public string? Latitude { get; set; }
    public string? Longitude { get; set; }
    public Guid? StateId { get; set; }
}
