using HRM.API.Domain.Common.Contracts;
using Microsoft.AspNetCore.Identity;

namespace HRM.API.Infrastructure.Identity;

public class ApplicationUser : IdentityUser, ISoftDelete
{
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? ImageUrl { get; set; }
    public bool IsActive { get; set; }
    public bool IsPrimaryUser { get; set; }
    public string? RefreshToken { get; set; }
    public DateTime RefreshTokenExpiryTime { get; set; }
    public string? ObjectId { get; set; }
    public string? StoreId { get; set; }
    public DateTime? CreatedOn { get; set; }
    public Guid? DeletedBy { get; set; }
    public DateTime? DeletedOn { get; set; }
}