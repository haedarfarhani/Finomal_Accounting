using Finomal.Application.Abstractions;
using Finomal.Application.Users.Dtos;


namespace Finomal.Application.Users.ChangePassword
{
    public record ChangePasswordCommand(
        string UserId,
        string CurrentPassword,
        string NewPassword
    ) : ICommand<ChangePasswordResponseDto>;
}
