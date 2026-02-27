namespace Finomal.Application.Users.Dtos
{
    public class UserResponseDto
    {
        public string Id { get; init; } = string.Empty;
        public string UserName { get; init; } = string.Empty;

        public string? FirstName { get; init; }
        public string? LastName { get; init; }
        public string FullName => string.Join(" ", new[] { FirstName, LastName }.Where(s => !string.IsNullOrWhiteSpace(s))).Trim();

        public string? Email { get; init; }
        public bool EmailConfirmed { get; init; }

        public string? PhoneNumber { get; init; }
        public bool PhoneNumberConfirmed { get; init; }

        public IReadOnlyList<string> Roles { get; init; } = Array.Empty<string>();

        public bool IsActive { get; init; }
        public bool TwoFactorEnabled { get; init; }
        public bool IsLockedOut { get; init; }
        public DateTime? LockoutEndDate { get; init; }

        public DateTime CreatedDate { get; init; }
        public DateTime? LastLoginDate { get; init; }
        public DateTime? LastActivityDate { get; init; }

        public int AccessFailedCount { get; init; }
    }
}
