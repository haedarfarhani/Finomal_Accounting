namespace Finomal.Application.Users.Dtos
{
    public class ChangePasswordResponseDto
    {
        public string UserId { get; init; } = string.Empty;
        public DateTime ChangedAt { get; init; }
        public bool Success { get; init; }
        public string Message { get; init; } = string.Empty;
    }
}
