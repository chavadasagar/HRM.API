using MasterPOS.API.Domain.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MasterPOS.API.Application.Identity.Customers;
public class CustomerByEmailSpec : Specification<Customer>, ISingleResultSpecification
{
    public CustomerByEmailSpec(string email)
    {
        Query.Where(b => b.Email.Trim().ToLower() == email.Trim().ToLower());
    }
}