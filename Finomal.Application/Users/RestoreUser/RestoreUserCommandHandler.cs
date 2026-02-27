using Finomal.Application.Abstractions;
using Finomal.Application.Users.Dtos;
using Finomal.Domain.Abstractions;
using Finomal.Domain.Users;

namespace Finomal.Application.Users.RestoreUser
{
    public class RestoreUserCommandHandler : ICommandHandler<RestoreUserCommand, UserResponseDto>
    {
        private readonly IUserRepository _userRepository;

        public RestoreUserCommandHandler(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<Result<UserResponseDto>> Handle(RestoreUserCommand request, CancellationToken cancellationToken)
        {
            if (!Guid.TryParse(request.UserId, out var userId))
            {
                return Result.Fail<UserResponseDto>("شناسه کاربر نامعتبر است.");
            }

            // کاربر را با روابط لود می‌کنیم تا DTO درست پر شود
            var user = await _userRepository.GetByIdAsync(userId);
            if (user == null)
            {
                return Result.Fail<UserResponseDto>("کاربر با این شناسه یافت نشد.");
            }

            if (!user.IsDeleted)
            {
                return Result.Fail<UserResponseDto>("این کاربر حذف نشده است و نیازی به بازگردانی ندارد.");
            }

            await _userRepository.RestoreAsync(userId);

            // بارگذاری مجدد بعد از restore (اگر متد RestoreAsync فقط IsDeleted را false می‌کند)
            // بعضی پیاده‌سازی‌ها نیاز به reload دارند تا تغییرات منعکس شود
            user = await _userRepository.GetByIdAsync(userId);

            var dto = new UserResponseDto
            {
                Id = user.Id.ToString(),
                UserName = user.UserName,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,

                Roles = user.UserRoles?
                    .Select(ur => ur.Role?.Name ?? "")
                    .Where(r => !string.IsNullOrWhiteSpace(r))
                    .ToList()
                    .AsReadOnly() ?? Array.Empty<string>().AsReadOnly(),

                IsActive = user.IsActive,
                // اگر فیلد IsDeleted در DTO دارید:
                // IsDeleted            = user.IsDeleted,

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
