namespace MasterPOS.API.Application.Catalog;
public class BrandDropdownDto : IDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = default!;
}
