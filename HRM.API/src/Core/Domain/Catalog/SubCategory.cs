namespace HRM.API.Domain.Catalog;
public class SubCategory : AuditableEntity, IAggregateRoot
{
    public string Name { get; private set; }
    public string? Description { get; private set; }
    public string? ImagePath { get; private set; }
    public bool IsActive { get; set; }
    public Guid? CategoryId { get; set; }
    public virtual Category Category { get; set; }

    public SubCategory(string name, string? description, bool isActive, string? imagePath)
    {
        Name = name;
        Description = description;
        IsActive = isActive;
        ImagePath = imagePath;
    }

    public SubCategory Update(string? name, string? description, bool isActive, string? imagePath)
    {
        if (name is not null && Name?.Equals(name) is not true) Name = name;
        if (description is not null && Description?.Equals(description) is not true) Description = description;
        IsActive = isActive;
        if (imagePath is not null && ImagePath?.Equals(imagePath) is not true) ImagePath = imagePath;
        return this;
    }

    public SubCategory ClearImagePath()
    {
        ImagePath = string.Empty;
        return this;
    }
}
