using MasterPOS.API.Domain.Identity;

namespace MasterPOS.API.Application.Identity.Customers;
public class CustomerByNameSpec : Specification<Customer>, ISingleResultSpecification
{
    public CustomerByNameSpec(string name) =>
        Query.Where(b => b.Name == name);
}