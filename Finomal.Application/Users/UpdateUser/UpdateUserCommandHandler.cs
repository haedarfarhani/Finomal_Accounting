using Finomal.Application.Abstractions;
using Finomal.Application.Users.Dtos;
using Finomal.Domain.Abstractions;
using Finomal.Domain.Users;

namespace Finomal.Application.Users.UpdateUser
{
    public class UpdateUserCommandHandler : ICommandHandler<UpdateUserCommand, UserResponseDto>
    {
        private readonly IUserRepository _userRepository;

        public UpdateUserCommandHandler(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<Result<UserResponseDto>> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
        {
            if (!Guid.TryParse(request.UserId, out var userId))
                return Result.Fail<UserResponseDto>("شناسه کاربر نامعتبر است.");

            var user = await _userRepository.GetByIdAsync(userId);
            if (user is null)
                return Result.Fail<UserResponseDto>("کاربر پیدا نشد.");

            // ── بررسی تکراری نبودن ایمیل (اگر تغییر کرده) ──
            if (!string.IsNullOrWhiteSpace(request.Email) &&
                !string.Equals(user.NormalizedEmail, request.Email.Trim().ToUpperInvariant(), StringComparison.OrdinalIgnoreCase) &&
                await _userRepository.ExistsByEmailAsync(request.Email))
            {
                return Result.Fail<UserResponseDto>("این ایمیل قبلاً برای کاربر دیگری ثبت شده است.");
            }

            // ── اعمال تغییرات پایه ──
            user.FirstName = request.FirstName?.Trim() ?? user.FirstName;
            user.LastName = request.LastName?.Trim() ?? user.LastName;
            user.Email = request.Email?.Trim();
            user.NormalizedEmail = request.Email?.Trim().ToUpperInvariant();
            user.PhoneNumber = request.PhoneNumber?.Trim();

            // اگر UserName هم قابل تغییر است (معمولاً در سیستم‌های هویتی محدود است)
            if (!string.IsNullOrWhiteSpace(request.UserName) &&
                !string.Equals(user.UserName, request.UserName.Trim(), StringComparison.OrdinalIgnoreCase))
            {
                if (await _userRepository.ExistsByUserNameAsync(request.UserName))
                    return Result.Fail<UserResponseDto>("این نام کاربری قبلاً استفاده شده است.");

                user.UserName = request.UserName.Trim();
                user.NormalizedUserName = request.UserName.Trim().ToUpperInvariant();
            }

            // ── مدیریت نقش‌ها ──
            // نکته: اینجا فرض می‌کنیم request فقط یک RoleId می‌فرستد (رایج‌ترین حالت در فرم‌های ساده)
            if (!string.IsNullOrWhiteSpace(request.RoleId))
            {
                if (!Guid.TryParse(request.RoleId, out var newRoleId))
                    return Result.Fail<UserResponseDto>("شناسه نقش نامعتبر است.");

                // حذف نقش‌های فعلی (اگر سیاست شما جایگزینی کامل است)
                foreach (var userRole in user.UserRoles.ToList())
                {
                    await _userRepository.RemoveUserFromRoleAsync(userId, userRole.RoleId);
                }

                // افزودن نقش جدید
                await _userRepository.AddUserToRoleAsync(userId, newRoleId);
            }

            // اگر می‌خواهید امکان ارسال لیست نقش‌ها را داشته باشید، باید Command را تغییر دهید

            await _userRepository.UpdateAsync(user);

            // بارگذاری مجدد برای گرفتن نقش‌های به‌روز شده (اگر لازم است)
            // var updatedUser = await _userRepository.GetByIdAsync(userId, includeRoles: true);

            return Result.OK(new UserResponseDto
            {
                Id = user.Id.ToString(),
                UserName = user.UserName,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                EmailConfirmed = user.EmailConfirmed,
                PhoneNumber = user.PhoneNumber,
                PhoneNumberConfirmed = user.PhoneNumberConfirmed,
                Roles = user.UserRoles.Select(ur => ur.Role?.Name)
                                        .Where(name => !string.IsNullOrWhiteSpace(name))
                                        .ToList()
                                        .AsReadOnly(),
                IsActive = user.IsActive,
                TwoFactorEnabled = user.TwoFactorEnabled,
                IsLockedOut = user.IsLockedOut || (user.LockoutEndDate.HasValue && user.LockoutEndDate > DateTime.UtcNow),
                LockoutEndDate = user.LockoutEndDate,
                CreatedDate = user.CreatedDate,
                LastLoginDate = user.LastLoginDate,
                LastActivityDate = user.LastActivityDate,
                AccessFailedCount = user.AccessFailedCount,
            });
        }
    }
}
