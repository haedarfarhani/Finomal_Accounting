using Finomal.Application.Abstractions;
using Finomal.Application.Users.Dtos;

namespace Finomal.Application.Users.GetUsersRole
{
    public record GetUsersByRoleQuery(string RoleId) : IQuery<List<UserDto>>;
}
