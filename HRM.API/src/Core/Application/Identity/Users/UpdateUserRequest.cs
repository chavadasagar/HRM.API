namespace MasterPOS.API.Application.Identity.Users;

public class UpdateUserRequest
{
    public string Id { get; set; } = default!;
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? PhoneNumber { get; set; }
    public string? Email { get; set; }
    public FileUploadRequest? Image { get; set; }
    public bool DeleteCurrentImage { get; set; } = false;
    public string? StoreId { get; set; }
    public string? RoleId { get; set; }
    public string? Password { get; set; }
    public bool IsActive { get; set; } = true;
}