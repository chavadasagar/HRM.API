using HRM.API.Domain.Configuration;

namespace HRM.API.Domain.Catalog;
public class Client : AuditableEntity, IAggregateRoot
{
    public Client(string? firstname, string? lastname, string? username, string? email, string? clientId, string? phone, DefaultIdType? companyId)
    {
        Firstname = firstname;
        Lastname = lastname;
        Username = username;
        Email = email;
        ClientId = clientId;
        Phone = phone;
        CompanyId = companyId;
    }

    public string? Firstname { get; set; }
    public string? Lastname { get; set; }
    public string? Username { get; set; }
    public string? Email { get; set; }
    public string? ClientId { get; set; }
    public string? Phone { get; set; }
    public Guid? CompanyId { get; set; }
    public virtual Company? Company { get; set; }
}
