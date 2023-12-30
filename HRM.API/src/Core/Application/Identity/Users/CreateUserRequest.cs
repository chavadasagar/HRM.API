namespace HRM.API.Application.Identity.Users;

public class CreateUserRequest
{
    public string FirstName { get; set; } = default!;
    public string LastName { get; set; } = default!;
    public string Email { get; set; } = default!;
    public string UserName { get; set; } = default!;
    public string Password { get; set; } = default!;
    public string ConfirmPassword { get; set; } = default!;
    public string? PhoneNumber { get; set; }
    public string? StoreId { get; set; }
    public string? RoleId { get; set; }
    public FileUploadRequest? Image { get; set; }
    public bool IsActive { get; set; } = true;
}