namespace MasterPOS.API.Application.Identity.Users;

public class UserDetailsDto
{
    public Guid Id { get; set; }

    public string? UserName { get; set; }

    public string? FirstName { get; set; }

    public string? LastName { get; set; }

    public string? Email { get; set; }
    public bool IsActive { get; set; } = true;
    public bool IsPrimaryUser { get; set; } = false;
    public bool EmailConfirmed { get; set; }
    public string? PhoneNumber { get; set; }
    public string? ImageUrl { get; set; }
    public string? RoleId { get; set; }
    public string? RoleName { get; set; }
    public string? StoreId { get; set; }
    public DateTime? CreatedOn { get; set; }
}