using Finomal.Application.Abstractions;
using Finomal.Application.Users.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Finomal.Application.Users.DeleteUser
{
    public record DeleteUserCommand(
        string UserId
    ) : ICommand<UserResponseDto>;
}
