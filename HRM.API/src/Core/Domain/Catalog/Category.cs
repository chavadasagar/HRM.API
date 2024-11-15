namespace MasterPOS.API.Domain.Catalog;
public class Category : AuditableEntity, IAggregateRoot
{
    public string Name { get; private set; }
    public string? Description { get; private set; }
    public string? ImagePath { get; private set; }
    public bool IsActive { get; set; }

    public Category(string name, string? description, bool isActive, string? imagePath)
    {
        Name = name;
        Description = description;
        IsActive = isActive;
        ImagePath = imagePath;
    }

    public Category Update(string? name, string? description, bool isActive, string? imagePath)
    {
        if (name is not null && Name?.Equals(name) is not true) Name = name;
        if (description is not null && Description?.Equals(description) is not true) Description = description;
        IsActive = isActive;
        if (imagePath is not null && ImagePath?.Equals(imagePath) is not true) ImagePath = imagePath;
        return this;
    }

    public Category ClearImagePath()
    {
        ImagePath = string.Empty;
        return this;
    }
}