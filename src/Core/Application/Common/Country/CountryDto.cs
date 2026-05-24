namespace HRM.API.Application.Common;
public class CountryDto : IDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = default!;
}
