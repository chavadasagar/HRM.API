namespace HRM.API.Application.Catalog.Suppliers;
public class SupplierDropdownDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = default!;
    public string? Mobile { get; private set; }
}
