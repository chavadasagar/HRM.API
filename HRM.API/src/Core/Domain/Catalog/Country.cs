namespace HRM.API.Domain.Catalog;
public class Country : BaseEntity, IAggregateRoot
{
    public string Name { get; set; }
    public string NormalizedName { get; set; }
    public Country(string name)
    {
        Name = name;
        NormalizedName = name.ToUpperInvariant();
    }
}
