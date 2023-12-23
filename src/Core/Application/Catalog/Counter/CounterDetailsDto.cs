using HRM.API.Application.Catalog.Stores;

namespace HRM.API.Application.Catalog.Counters;

public class CounterDetailsDto : IDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = default!;
    public string? Description { get; set; }
    public bool IsDefault { get; private set; }
    public bool IsActive { get; private set; }
    public Guid StoreId { get; set; }
    public string? StoreName { get; set; }
    public string? StoreCode { get; set; }
}