namespace HRM.API.Application.Catalog;
public class CategoryDto : IDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = default!;
    public string? Description { get; set; }
    public string? ImagePath { get; set; }
    public bool IsActive { get; set; }
}