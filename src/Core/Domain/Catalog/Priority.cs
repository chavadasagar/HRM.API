using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRM.API.Domain.Catalog;
public class Priority : AuditableEntity, IAggregateRoot
{
    public Priority(string? name, string? description)
    {
        Name = name;
        Description = description;
    }

    public string? Name { get; set; } = "Medium";
    public string? Description { get; set; }
}
