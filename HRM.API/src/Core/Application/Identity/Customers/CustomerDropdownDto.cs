namespace MasterPOS.API.Application.Identity.Customers;
public class CustomerDropdownDto
{
    public Guid Id { get; set; }
    public string Name { get; private set; } = default!;
}
