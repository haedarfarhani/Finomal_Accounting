using Finomal.Application.Abstractions;
using Finomal.Application.Users.Dtos;
using Finomal.Domain.Abstractions;
using Finomal.Domain.Users;

namespace Finomal.Application.Users.ChangeUserRole
{
    public class ChangeUserRoleCommandHandler : ICommandHandler<ChangeUserRoleCommand, UserResponseDto>
    {
        private readonly IUserRepository _userRepository;

        public ChangeUserRoleCommandHandler(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<Result<UserResponseDto>> Handle(
            ChangeUserRoleCommand request,
            CancellationToken cancellationToken)
        {
            if (!Guid.TryParse(request.UserId, out var userId))
                return Result.Fail<UserResponseDto>("شناسه کاربر نامعتبر است.");

            if (!Guid.TryParse(request.OldRoleId, out var oldRoleId))
                return Result.Fail<UserResponseDto>("شناسه نقش قدیمی نامعتبر است.");

            if (!Guid.TryParse(request.NewRoleId, out var newRoleId))
                return Result.Fail<UserResponseDto>("شناسه نقش جدید نامعتبر است.");

            if (oldRoleId == newRoleId)
                return Result.Fail<UserResponseDto>("نقش قدیمی و جدید یکسان است.");

            // لود کاربر + نقش‌هایش
            var user = await _userRepository.GetByIdAsync(userId);
            if (user == null)
                return Result.Fail<UserResponseDto>("کاربر پیدا نشد.");

            // اختیاری: جلوگیری از تغییر نقش برخی کاربران حساس
            // if (user.IsSuperAdmin || user.Id == someProtectedId)
            //     return Result.Fail<UserResponseDto>("نمی‌توانید نقش این کاربر را تغییر دهید.");

            // چک وجود نقش جدید
            var newRole = await _userRepository.GetRoleByIdAsync(newRoleId);
            if (newRole == null)
                return Result.Fail<UserResponseDto>("نقش جدید یافت نشد.");

            // چک کنیم آیا واقعاً نقش قدیمی را دارد یا نه
            var hasOldRole = user.UserRoles?.Any(ur => ur.RoleId == oldRoleId) == true;
            if (!hasOldRole)
            {
                // می‌توانید اینجا Fail کنید یا فقط بی‌صدا رد شوید (بسته به نیاز)
                // return Result.Fail<UserResponseDto>("کاربر این نقش قدیمی را نداشت.");
            }

            // حذف نقش قدیمی (اگر وجود داشت)
            if (hasOldRole)
            {
                await _userRepository.RemoveUserFromRoleAsync(userId, oldRoleId);
            }

            // افزودن نقش جدید
            await _userRepository.AddUserToRoleAsync(userId, newRoleId);

            // آپدیت نهایی کاربر (اگر نیاز به تغییر فیلد دیگری باشد)
            await _userRepository.UpdateAsync(user);

            // بارگذاری مجدد برای اطمینان از به‌روز بودن (در برخی پیاده‌سازی‌ها لازم است)
            var updatedUser = await _userRepository.GetByIdAsync(userId);

            var response = new UserResponseDto
            {
                Id = updatedUser.Id.ToString(),
                UserName = updatedUser.UserName,
                FirstName = updatedUser.FirstName,
                LastName = updatedUser.LastName,
                Email = updatedUser.Email,
                PhoneNumber = updatedUser.PhoneNumber,

                Roles = updatedUser.UserRoles?
                    .Select(ur => ur.Role?.Name ?? "")
                    .Where(r => !string.IsNullOrWhiteSpace(r))
                    .ToList()
                    .AsReadOnly() ?? Array.Empty<string>().AsReadOnly(),

                IsActive = updatedUser.IsActive,
                TwoFactorEnabled = updatedUser.TwoFactorEnabled,
                IsLockedOut = updatedUser.IsLockedOut || (updatedUser.LockoutEndDate.HasValue && updatedUser.LockoutEndDate > DateTime.UtcNow),
                LockoutEndDate = updatedUser.LockoutEndDate,
                CreatedDate = updatedUser.CreatedDate,
                LastLoginDate = updatedUser.LastLoginDate,
            };

            return Result.OK(response);
        }
    }
}