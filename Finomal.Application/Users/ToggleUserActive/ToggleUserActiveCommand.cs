using Finomal.Application.Abstractions;
using Finomal.Application.Users.Dtos;

namespace Finomal.Application.Users.Commands
{
    public record ToggleUserActiveCommand(string UserId, bool IsActive) : ICommand<UserResponseDto>;
}