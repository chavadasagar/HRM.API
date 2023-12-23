namespace HRM.API.Application.Catalog;
public class CategoryDropdownDto : IDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = default!;
}
