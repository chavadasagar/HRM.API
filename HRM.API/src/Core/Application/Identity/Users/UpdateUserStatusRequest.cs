using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MasterPOS.API.Application.Identity.Users;
public class UpdateUserStatusRequest
{
    public string Id { get; set; } = default!;
    public bool IsActive { get; set; } = true;
}
