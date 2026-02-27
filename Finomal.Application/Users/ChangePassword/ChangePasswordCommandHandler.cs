using Finomal.Application.Abstractions;
using Finomal.Application.Users.Dtos;
using Finomal.Domain.Abstractions;
using Finomal.Domain.Users;

namespace Finomal.Application.Users.ChangePassword
{
    public class ChangePasswordCommandHandler : ICommandHandler<ChangePasswordCommand, ChangePasswordResponseDto>
    {
        private readonly IUserRepository _userRepository;
        private readonly IPasswordHasher _passwordHasher;

        public ChangePasswordCommandHandler(IUserRepository userRepository,IPasswordHasher passwordHasher)
        {
            _userRepository = userRepository;
            _passwordHasher = passwordHasher;
        }

        public async Task<Result<ChangePasswordResponseDto>> Handle(ChangePasswordCommand request,CancellationToken cancellationToken)
        {
            if (!Guid.TryParse(request.UserId, out var userId))
                return Result.Fail<ChangePasswordResponseDto>("شناسه کاربر نامعتبر است.");

            var user = await _userRepository.GetByIdAsync(userId);
            if (user is null)
                return Result.Fail<ChangePasswordResponseDto>("کاربر پیدا نشد.");

            if (string.IsNullOrWhiteSpace(request.CurrentPassword))
                return Result.Fail<ChangePasswordResponseDto>("رمز عبور فعلی وارد نشده است.");

            if (!_passwordHasher.Verify(request.CurrentPassword, user.PasswordHash))
                return Result.Fail<ChangePasswordResponseDto>("رمز عبور فعلی اشتباه است.");

            if (string.IsNullOrWhiteSpace(request.NewPassword))
                return Result.Fail<ChangePasswordResponseDto>("رمز عبور جدید وارد نشده است.");

            user.PasswordHash = _passwordHasher.Hash(request.NewPassword);

            // اختیاری: invalidate توکن‌های قبلی
            // user.SecurityStamp = Guid.NewGuid().ToString();

            await _userRepository.UpdateAsync(user);

            return Result.OK(new ChangePasswordResponseDto
            {
                UserId = userId.ToString(),
                ChangedAt = DateTime.UtcNow,
                Success = true,
                Message = "رمز عبور با موفقیت تغییر یافت."
            });
        }
    }
}
