using Finomal.Application.Abstractions;
using Finomal.Application.Users.Dtos;
using Finomal.Domain.Abstractions;


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
            // 1. اعتبارسنجی ورودی
            if (string.IsNullOrWhiteSpace(request.UserName) ||
                string.IsNullOrWhiteSpace(request.Password) ||
                (request.RoleId == 0)) 
            {
                return Result.Fail<LoginDto>("نام کاربری، رمز عبور و نقش نمی‌توانند خالی باشند.");
            }

            // 2. فراخوانی سرویس احراز هویت برای انجام منطق ورود
            // فرض می‌کنیم LoginUserAsync حالا نقش را هم می‌گیرد یا حداقل Role را در LoginDto برمی‌گرداند.
            // اگر سرویس شما نقش را نمی‌گیرد و فقط Username/Password را چک می‌کند و سپس نقش را از دیتابیس می‌خواند،
            // نیاز نیست اینجا آن را به LoginUserAsync پاس دهید، فقط مطمئن شوید که LoginDto برگشتی شامل Role است.
            var loginDto = await _authenticationService.LoginUserAsync(request.UserName, request.Password, request.RoleId);

            // 3. بررسی موفقیت عملیات ورود
            if (!loginDto.IsSuccess)
            {
                return Result.Fail<LoginDto>(loginDto.Message); // <--- Type parameter changed
            }

            // 5. برگرداندن پاسخ موفقیت‌آمیز
            return Result.OK(loginDto); 
        }
    }
}
