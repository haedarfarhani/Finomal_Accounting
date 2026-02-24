using Finomal.Application.Abstractions;
using Finomal.Application.Users.Dtos;
using Finomal.Domain.Abstractions;
using Finomal.Domain.Users;

namespace Finomal.Application.Users.Register
{
    public class RegisterCommandHandler : ICommandHandler<RegisterCommand, RegisterResponseDto>
    {
        private readonly IUserRepository _userRepository;
        private readonly IAuthenticationService _authenticationService;

        public RegisterCommandHandler(IUserRepository userRepository, IAuthenticationService authenticationService)
        {
            _userRepository = userRepository;
            _authenticationService = authenticationService;
        }

        public async Task<Result<RegisterResponseDto>> Handle(RegisterCommand request, CancellationToken cancellationToken)
        {
            // 1. اعتبارسنجی ورودی
            if (string.IsNullOrWhiteSpace(request.UserName) ||
                string.IsNullOrWhiteSpace(request.Password) ||
                string.IsNullOrWhiteSpace(request.Email) ||
                request.RoleId == null) 
            {
                return Result.Fail<RegisterResponseDto>("همه فیلدهای ضروری باید پر شوند.");
            }

            // 2. بررسی وجود نام کاربری/ایمیل تکراری
            var existingUser = await _userRepository.GetByUserNameAsync(request.UserName);
            if (existingUser != null)
            {
                return Result.Fail<RegisterResponseDto>("نام کاربری قبلاً وجود دارد.");
            }
            // توصیه می‌شود که بررسی ایمیل تکراری را هم اضافه کنید
            //var existingEmailUser = await _userRepository.GetUserByEmailAsync(request.Email);
            //if (existingEmailUser != null)
            //{
            //    return Result.Fail<RegisterResponseDto>("ایمیل قبلاً ثبت شده است.");
            //}

            // 3. یافتن نقش
            var role = await _userRepository.GetRoleByIdAsync(request.RoleId); // از .Value برای گرفتن مقدار از nullable استفاده کنید
            if (role == null)
            {
                return Result.Fail<RegisterResponseDto>("نقش مشخص شده یافت نشد.");
            }

            // 4. Hash کردن رمز عبور
            var passwordHash = _authenticationService.GeneratePasswordHash(request.Password);

            // 5. ایجاد کاربر جدید
            var newUser = new User
            {
                RefreshToken = null, 
                RefreshTokenExpiryTime = null,
                UserName = request.UserName,
                PasswordHash = passwordHash,
                Email = request.Email,
                FirstName = request.FirstName,
                LastName = request.LastName,
                CreatedDate = DateTime.Now,
                IsActive = false, // کاربر جدید به طور پیش‌فرض فعال است (طبق منطق کسب و کار شما)
                LoginAttempts = 0,
                IsLockedOut = true, // کاربر جدید به طور پیش‌فرض قفل نیست
                // UserRoles را اینجا مقداردهی اولیه نکنید، در مرحله بعدی اضافه می‌شود
                UserRoles = new List<UserRole>() // مقداردهی اولیه به یک لیست خالی
            };

            // 6. اختصاص نقش به کاربر
            newUser.UserRoles.Add(new UserRole { User = newUser, Role = role });

            // 7. ذخیره کاربر در دیتابیس
            await _userRepository.AddAsync(newUser);

            // 8. برگرداندن پاسخ موفقیت‌آمیز
            return Result.OK(new RegisterResponseDto
            {
                IsSuccess = true,
                Message = "ثبت‌نام با موفقیت انجام شد.",
                UserId = newUser.Id,
                UserName = newUser.UserName
            });
        }
    }
}
