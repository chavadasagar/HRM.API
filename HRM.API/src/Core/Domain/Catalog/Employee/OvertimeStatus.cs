using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRM.API.Domain.Catalog;
public class OvertimeStatus : AuditableEntity, IAggregateRoot
{
    public OvertimeStatus(string? name, string? description)
    {
        Name = name;
        Description = description;
    }

    public string? Name { get; set; }
    public string? Description { get; set; }
}