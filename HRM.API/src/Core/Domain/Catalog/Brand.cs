namespace HRM.API.Domain.Catalog;

public class Brand : AuditableEntity, IAggregateRoot
{
    public string Name { get; private set; }
    public string? Description { get; private set; }
    public string? ImagePath { get; private set; }
    public bool IsActive { get; set; }

    public Brand(string name, string? description, bool isActive, string? imagePath)
    {
        Name = name;
        Description = description;
        IsActive = isActive;
        ImagePath = imagePath;
    }

    public Brand Update(string? name, string? description, bool isActive, string? imagePath)
    {
        if (name is not null && Name?.Equals(name) is not true) Name = name;
        if (description is not null && Description?.Equals(description) is not true) Description = description;
        IsActive = isActive;
        if (imagePath is not null && ImagePath?.Equals(imagePath) is not true) ImagePath = imagePath;
        return this;
    }

    public Brand ClearImagePath()
    {
        ImagePath = string.Empty;
        return this;
    }
}