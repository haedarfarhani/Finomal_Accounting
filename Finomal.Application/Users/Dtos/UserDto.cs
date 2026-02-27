namespace Finomal.Application.Users.Dtos
{
    public record UserDto(
        string Id,
        string UserName,
        string? Email,
        bool EmailConfirmed,
        string? PhoneNumber,
        bool PhoneNumberConfirmed,
        string FullName,
        string? FirstName,
        string? LastName,
        IReadOnlyList<string> Roles,
        bool IsActive,
        bool IsLockedOut,
        DateTime CreatedDate,
        DateTime? LastLoginDate,
        DateTime? LastActivityDate = null
    );
}