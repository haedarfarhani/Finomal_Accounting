using Finomal.Application.Abstractions;
using Finomal.Application.Users.Dtos;

namespace Finomal.Application.Users.ResetPassword
{
    public record ResetPasswordCommand(
        string UserId,
        string NewPassword
    ) : ICommand<ChangePasswordResponseDto>;
}
