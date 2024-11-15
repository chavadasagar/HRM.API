namespace MasterPOS.API.Application.Catalog;
public class UnitDropdownDto : IDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = default!;
}
