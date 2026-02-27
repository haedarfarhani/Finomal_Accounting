using Finomal.Application.Abstractions;
using Finomal.Application.Users.Dtos;

namespace Finomal.Application.Users.CreateUser
{
    public record CreateUserCommand(
        string FirstName,
        string LastName,
        string UserName,
        string? Email,
        string? PhoneNumber,
        string Password,
        string RoleId
    ) : ICommand<UserResponseDto>;
}
