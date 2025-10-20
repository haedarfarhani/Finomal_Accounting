using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Finomal.Application.Users
{
    public record struct UserDto
 (
     string Id,
     string UserName,
     string Email,
     string Roles
 )
    { }
}
