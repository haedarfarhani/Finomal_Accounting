using Finomal.Application.Abstractions;
using Finomal.Application.Users.Dtos;

namespace Finomal.Application.Users.ChangeUserRole
{
    public record ChangeUserRoleCommand(
        string UserId,
        string OldRoleId,
        string NewRoleId
    ) : ICommand<UserResponseDto>;
}
