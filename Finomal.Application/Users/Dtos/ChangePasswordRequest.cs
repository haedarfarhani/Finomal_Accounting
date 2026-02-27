namespace Finomal.Application.Users.Dtos
{
    public record ChangePasswordRequest(
        string UserId,
        string CurrentPassword,
        string NewPassword
    );
}
