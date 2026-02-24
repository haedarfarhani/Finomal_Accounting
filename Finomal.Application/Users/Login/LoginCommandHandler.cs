using Finomal.Application.Abstractions;
using Finomal.Application.Users.Dtos;
using Finomal.Domain.Abstractions;
using MediatR;

namespace Finomal.Application.Users.Login
{
    public class LoginCommandHandler : ICommandHandler<LoginCommand, LoginDto>
    {
        private readonly IAuthenticationService _authenticationService;

        public LoginCommandHandler(IAuthenticationService authenticationService)
        {
            _authenticationService = authenticationService;
        }

        public async Task<Result<LoginDto>> Handle(LoginCommand request, CancellationToken cancellationToken)
        {
            // ۱. اعتبارسنجی اولیه ورودی
            if (string.IsNullOrWhiteSpace(request.UserName))
                return Result.Fail<LoginDto>("نام کاربری نمی‌تواند خالی باشد.");

            if (string.IsNullOrWhiteSpace(request.Password))
                return Result.Fail<LoginDto>("رمز عبور نمی‌تواند خالی باشد.");

            if (request.RoleId == null)
                return Result.Fail<LoginDto>("نقش معتبر انتخاب نشده است.");

            // ۲. فراخوانی سرویس احراز هویت
            var loginResult = await _authenticationService.LoginUserAsync(
                request.UserName,
                request.Password,
                request.RoleId);

            // ۳. اگر سرویس خودش Result برگردانده، مستقیم پاس بده
            return loginResult;
        }
    }
}