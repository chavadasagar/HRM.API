namespace MasterPOS.API.Domain.Catalog;

public class Counter : AuditableEntity, IAggregateRoot
{
    public string Name { get; private set; } = default!;
    public string? Description { get; private set; }
    public Guid StoreId { get; private set; }
    public bool IsDefault { get; private set; }
    public bool IsActive { get; private set; }
    public virtual Store Store { get; private set; } = default!;

    public Counter(string name, string? description,  Guid storeId,  bool isDefault, bool isActive)
    {
        Name = name;
        Description = description;
        StoreId = storeId;
        IsDefault = isDefault;
        IsActive = isActive;
    }

    public Counter Update(string? name, string? description, Guid? storeId, bool? isDefault, bool? isActive)
    {
        if (name is not null && Name?.Equals(name) is not true) Name = name;
        if (description is not null && Description?.Equals(description) is not true) Description = description;
        if (storeId.HasValue && storeId.Value != Guid.Empty && !StoreId.Equals(storeId.Value)) StoreId = storeId.Value;
        if (isDefault.HasValue && !IsDefault.Equals(isDefault.Value)) IsDefault = isDefault.Value;
        if (isActive.HasValue && !IsActive.Equals(isActive.Value)) IsActive = isActive.Value;
        return this;
    }

    public Counter UpdateStatus(bool isActive)
    {
        IsActive = isActive;
        return this;
    }
}