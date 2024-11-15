namespace HRM.API.Application.Catalog.Stores;
public class StoreDropdownDto
{
    public Guid Id { get; set; }
    public string Code { get; private set; } = default!;
    public string Name { get; private set; } = default!;
}
