namespace Finomal.Application.Users.Dtos
{
    public record ResetPasswordRequest(
        string UserId,
        string NewPassword
    );
}
