using Finomal.Application.Abstractions;
using Finomal.Application.Users.Dtos;
using Finomal.Domain.Abstractions;
using Finomal.Domain.Users;

namespace Finomal.Application.Users.DeleteUser
{
    public class DeleteUserCommandHandler : ICommandHandler<DeleteUserCommand, UserResponseDto>
    {
        private readonly IUserRepository _userRepository;

        public DeleteUserCommandHandler(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<Result<UserResponseDto>> Handle(
            DeleteUserCommand request,
            CancellationToken cancellationToken)
        {
            if (!Guid.TryParse(request.UserId, out var userId))
            {
                return Result.Fail<UserResponseDto>("شناسه کاربر نامعتبر است.");
            }

            // بهتر است روابط را لود کنیم تا DTO درست پر شود (اگر لازم است)
            var user = await _userRepository.GetByIdAsync(userId);
            if (user == null)
            {
                return Result.Fail<UserResponseDto>("کاربر پیدا نشد.");
            }

            // اختیاری: جلوگیری از حذف برخی کاربران حساس (مثل ادمین اصلی یا کاربر لاگین‌شده)
            // if (user.IsAdmin || user.Id == currentUserId) // نیاز به تزریق ICurrentUserService دارد
            // {
            //     return Result.Fail<UserResponseDto>("حذف این کاربر مجاز نیست.");
            // }

            // اختیاری: چک کردن اینکه آیا کاربر قبلاً حذف شده یا نه
            if (user.IsDeleted)
            {
                return Result.Fail<UserResponseDto>("کاربر قبلاً حذف شده است.");
            }

            await _userRepository.SoftDeleteAsync(userId);

            // ساخت DTO نهایی (حالت بعد از حذف)
            var response = new UserResponseDto
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
                CreatedDate = user.CreatedDate,
                LastLoginDate = user.LastLoginDate,
            };

            return Result.OK(response);
        }
    }
}
