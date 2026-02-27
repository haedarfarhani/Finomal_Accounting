using Finomal.Application.Abstractions;
using Finomal.Application.Users.Dtos;
using Finomal.Domain.Abstractions;
using Finomal.Domain.Users;

namespace Finomal.Application.Users.CreateUser
{
    public class CreateUserCommandHandler : ICommandHandler<CreateUserCommand, UserResponseDto>
    {
        private readonly IUserRepository _userRepository;
        private readonly IPasswordHasher _passwordHasher;

        public CreateUserCommandHandler(
            IUserRepository userRepository,
            IPasswordHasher passwordHasher)
        {
            _userRepository = userRepository;
            _passwordHasher = passwordHasher;
        }

        public async Task<Result<UserResponseDto>> Handle(
            CreateUserCommand request,
            CancellationToken cancellationToken)
        {
            // اعتبارسنجی‌های اولیه
            if (string.IsNullOrWhiteSpace(request.UserName))
                return Result.Fail<UserResponseDto>("نام کاربری الزامی است.");

            if (await _userRepository.ExistsByUserNameAsync(request.UserName.Trim()))
                return Result.Fail<UserResponseDto>("نام کاربری قبلاً استفاده شده است.");

            if (!string.IsNullOrWhiteSpace(request.Email) &&
                await _userRepository.ExistsByEmailAsync(request.Email.Trim()))
                return Result.Fail<UserResponseDto>("ایمیل وارد‌شده قبلاً ثبت شده است.");

            if (!Guid.TryParse(request.RoleId, out var roleGuid))
                return Result.Fail<UserResponseDto>("شناسه نقش نامعتبر است.");

            var role = await _userRepository.GetRoleByIdAsync(roleGuid);
            if (role is null)
                return Result.Fail<UserResponseDto>("نقش انتخاب‌شده وجود ندارد.");

            // اعتبارسنجی رمز عبور (اختیاری اما بسیار توصیه می‌شود)
            if (string.IsNullOrWhiteSpace(request.Password) || request.Password.Length < 8)
                return Result.Fail<UserResponseDto>("رمز عبور باید حداقل ۸ کاراکتر باشد.");

            // ساخت کاربر
            var user = new User
            {
                Id = Guid.NewGuid(),
                FirstName = request.FirstName?.Trim() ?? string.Empty,
                LastName = request.LastName?.Trim() ?? string.Empty,
                UserName = request.UserName.Trim(),
                NormalizedUserName = request.UserName.Trim().ToUpperInvariant(),
                Email = request.Email?.Trim(),
                NormalizedEmail = request.Email?.Trim().ToUpperInvariant(),
                PhoneNumber = request.PhoneNumber?.Trim(),
                PasswordHash = _passwordHasher.Hash(request.Password),
                IsActive = true,
                IsDeleted = false,
                CreatedDate = DateTime.UtcNow,
                // SecurityStamp = Guid.NewGuid().ToString(),   // اگر نیاز دارید
            };

            await _userRepository.AddAsync(user);
            await _userRepository.AddUserToRoleAsync(user.Id, roleGuid);

            // بارگذاری مجدد برای گرفتن نقش‌ها (اگر لازم است)
            var createdUser = await _userRepository.GetByIdAsync(user.Id);

            var dto = new UserResponseDto
            {
                Id = createdUser.Id.ToString(),
                UserName = createdUser.UserName,
                FirstName = createdUser.FirstName,
                LastName = createdUser.LastName,
                Email = createdUser.Email,
                PhoneNumber = createdUser.PhoneNumber,

                Roles = createdUser.UserRoles?
                    .Select(ur => ur.Role?.Name ?? "")
                    .Where(r => !string.IsNullOrWhiteSpace(r))
                    .ToList()
                    .AsReadOnly() ?? Array.Empty<string>().AsReadOnly(),

                IsActive = createdUser.IsActive,
                CreatedDate = createdUser.CreatedDate,
                // سایر فیلدهای مورد نیاز...
            };

            return Result.OK(dto);
        }
    }
}