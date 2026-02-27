using Finomal.Application.Abstractions;
using Finomal.Application.Users.Dtos;
using Finomal.Domain.Abstractions;
using Finomal.Domain.Users;

namespace Finomal.Application.Users.Commands
{
    public class ToggleUserActiveCommandHandler : ICommandHandler<ToggleUserActiveCommand, UserResponseDto>
    {
        private readonly IUserRepository _userRepository;

        public ToggleUserActiveCommandHandler(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<Result<UserResponseDto>> Handle(
            ToggleUserActiveCommand request,
            CancellationToken cancellationToken)
        {
            if (!Guid.TryParse(request.UserId, out var userId))
            {
                return Result.Fail<UserResponseDto>("شناسه کاربر نامعتبر است.");
            }

            // بهتر است روابط لازم را از ابتدا لود کنیم (برای پر شدن Roles در DTO)
            var user = await _userRepository.GetByIdAsync(userId);
            if (user == null)
            {
                return Result.Fail<UserResponseDto>("کاربر پیدا نشد.");
            }

            // اختیاری: جلوگیری از غیرفعال کردن برخی کاربران خاص (مثلاً خود ادمین اصلی)
            // if (user.Id == Guid.Parse("some-super-admin-id") && !request.IsActive)
            // {
            //     return Result.Fail<UserResponseDto>("نمی‌توانید حساب اصلی ادمین را غیرفعال کنید.");
            // }

            bool previousStatus = user.IsActive;
            user.IsActive = request.IsActive;

            // اختیاری: به‌روزرسانی زمان آخرین فعالیت (اگر سیاست پروژه دارید)
            // user.LastActivityDate = DateTime.UtcNow;

            await _userRepository.UpdateAsync(user);

            // ساخت پاسخ کامل و به‌روز
            var dto = new UserResponseDto
            {
                Id = user.Id.ToString(),
                UserName = user.UserName,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                EmailConfirmed = user.EmailConfirmed,
                PhoneNumber = user.PhoneNumber,
                PhoneNumberConfirmed = user.PhoneNumberConfirmed,

                Roles = user.UserRoles?
                    .Select(ur => ur.Role?.Name)
                    .Where(name => !string.IsNullOrWhiteSpace(name))
                    .ToList()
                    .AsReadOnly() ?? Array.Empty<string>().AsReadOnly(),

                IsActive = user.IsActive,
                TwoFactorEnabled = user.TwoFactorEnabled,
                IsLockedOut = user.IsLockedOut || (user.LockoutEndDate.HasValue && user.LockoutEndDate > DateTime.UtcNow),
                LockoutEndDate = user.LockoutEndDate,
                CreatedDate = user.CreatedDate,
                LastLoginDate = user.LastLoginDate,
                LastActivityDate = user.LastActivityDate,
                AccessFailedCount = user.AccessFailedCount,
            };

            return Result.OK(dto);
        }
    }
}