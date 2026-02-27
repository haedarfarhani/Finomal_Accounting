using Finomal.Application.Abstractions;
using Finomal.Application.Users.Dtos;
using Finomal.Domain.Abstractions;
using Finomal.Domain.Users;

namespace Finomal.Application.Users.ResetPassword
{
    public class ResetPasswordCommandHandler : ICommandHandler<ResetPasswordCommand, ChangePasswordResponseDto>
    {
        private readonly IUserRepository _userRepository;
        private readonly IPasswordHasher _passwordHasher;

        public ResetPasswordCommandHandler(IUserRepository userRepository, IPasswordHasher passwordHasher)
        {
            _userRepository = userRepository;
            _passwordHasher = passwordHasher;
        }

        public async Task<Result<ChangePasswordResponseDto>> Handle(
            ResetPasswordCommand request,
            CancellationToken cancellationToken)
        {
            if (!Guid.TryParse(request.UserId, out var guid))
                return Result.Fail<ChangePasswordResponseDto>("شناسه کاربر نامعتبر است.");

            var user = await _userRepository.GetByIdAsync(guid);
            if (user is null)
                return Result.Fail<ChangePasswordResponseDto>("کاربر پیدا نشد.");

            user.PasswordHash = _passwordHasher.Hash(request.NewPassword);
            await _userRepository.UpdateAsync(user);

            return Result.OK(new ChangePasswordResponseDto
            {
                UserId = request.UserId,
                ChangedAt = DateTime.UtcNow,
                Success = true,
                Message = "رمز عبور با موفقیت تغییر یافت."
            });
        }
    }
}
