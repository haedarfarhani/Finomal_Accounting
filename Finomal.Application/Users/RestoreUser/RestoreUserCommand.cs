using Finomal.Application.Abstractions;
using Finomal.Application.Users.Dtos;

namespace Finomal.Application.Users.RestoreUser
{
    public record RestoreUserCommand(
        string UserId
    ) : ICommand<UserResponseDto>;
}
