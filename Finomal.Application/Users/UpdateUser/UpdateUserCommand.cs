using Finomal.Application.Abstractions;
using Finomal.Application.Users.Dtos;

namespace Finomal.Application.Users.UpdateUser
{
    public record UpdateUserCommand(
        string UserId,
        string FirstName,
        string LastName,
        string? Email,
        string? PhoneNumber,
        string RoleId,
        string? UserName = null
    ) : ICommand<UserResponseDto>;
}
