using MasterPOS.API.Application.Inventory;

namespace MasterPOS.API.Application.Catalog.Suppliers;

public class SupplierDto : IDto
{
    public Guid Id { get; set; }
    public string Code { get; private set; } = default!;
    public string Name { get; private set; } = default!;
    public string? Mobile { get; private set; }
    public string? Email { get; private set; }
    public string? Phone { get; private set; }
    public string? GSTNumber { get; private set; }
    public DateTime? BirthDate { get; private set; }
    public DateTime? AnniversaryDate { get; private set; }
    public string? CountryName { get; private set; }
    public Guid? CountryId { get; private set; }
    public string? StateName { get; private set; }
    public Guid? StateId { get; private set; }
    public string? City { get; private set; }
    public string? Postcode { get; private set; }
    public string? Address { get; private set; }
    public string? SupplierLogoPath { get; private set; }
    public bool IsDefault { get; private set; }
    public bool IsPrimarySupplier { get; set; }
    public bool IsActive { get; private set; }
    public List<PurchaseDto>? PurchaseList { get; set; }
    public List<PurchaseReturnDto>? PurchaseReturnList { get; set; }
}