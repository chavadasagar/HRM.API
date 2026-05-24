using HRM.API.Domain.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRM.API.Application.Identity.Customers;
public class CustomerByMobileSpec : Specification<Customer>, ISingleResultSpecification
{
    public CustomerByMobileSpec(string mobileNo)
    {
        Query.Where(b => b.Mobile.Trim().ToLower() == mobileNo.Trim().ToLower());
    }
}