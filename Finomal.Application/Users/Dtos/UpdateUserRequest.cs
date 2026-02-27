namespace Finomal.Application.Users.Dtos
{
    public record UpdateUserRequest(
        string UserId,
        string FirstName,
        string LastName,
        string? Email,
        string? PhoneNumber,
        string RoleId
    );
}
