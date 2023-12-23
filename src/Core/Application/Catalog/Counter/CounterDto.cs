using HRM.API.Application.Catalog.Stores;

namespace HRM.API.Application.Catalog.Counters;

public class CounterDto : IDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = default!;
    public string? Description { get; set; }
    public bool IsDefault { get;  set; }
    public bool IsActive { get;  set; }
    public Guid StoreId { get; set; }
    public string? StoreCode { get;  set; }
    public string? StoreName { get;  set; }
    public Store Store { get; set; }
}