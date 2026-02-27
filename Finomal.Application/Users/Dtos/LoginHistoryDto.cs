namespace Finomal.Application.Users.Dtos
{
    public record LoginHistoryDto(
        string Id,
        DateTime EventTime,
        bool IsSuccess,
        string? IpAddress,
        string? UserAgent,
        string? FailReason
    );
}
