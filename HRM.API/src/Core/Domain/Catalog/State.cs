namespace MasterPOS.API.Domain.Catalog;
public class State : BaseEntity, IAggregateRoot
{
    public Guid CountryId { get; set; }
    public string Name { get; set; }
    public string NormalizedName { get; set; }
    public virtual Country Country { get; private set; } = default!;
    public State(string name, Guid countryId)
    {
        Name = name;
        NormalizedName = name.ToUpperInvariant();
        CountryId = countryId;
    }
}
