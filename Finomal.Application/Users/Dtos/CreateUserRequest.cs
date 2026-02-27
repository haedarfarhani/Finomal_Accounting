namespace Finomal.Application.Users.Dtos
{
    public record CreateUserRequest(
        string FirstName,
        string LastName,
        string UserName,
        string? Email,
        string? PhoneNumber,
        string Password,
        string RoleId
    );
}
