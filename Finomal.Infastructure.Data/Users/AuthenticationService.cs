using Finomal.Application.Users;
using Finomal.Application.Users.Dtos;
using Finomal.Domain.Abstractions;
using Finomal.Domain.Users;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace Finomal.Infrastructure.Data.Users
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly IUserRepository _userRepository;
        private readonly JwtSettings _jwtSettings;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public AuthenticationService(
            IUserRepository userRepository,
            IOptions<JwtSettings> jwtSettings,
            IHttpContextAccessor httpContextAccessor)
        {
            _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
            _jwtSettings = jwtSettings?.Value ?? throw new ArgumentNullException(nameof(jwtSettings));
            _httpContextAccessor = httpContextAccessor ?? throw new ArgumentNullException(nameof(httpContextAccessor));
        }

        public string GeneratePasswordHash(string password)
        {
            if (string.IsNullOrWhiteSpace(password))
                throw new ArgumentException("Password cannot be empty.", nameof(password));

            return BCrypt.Net.BCrypt.HashPassword(password, workFactor: 12);
        }

        public bool VerifyPassword(string password, string passwordHash)
        {
            if (string.IsNullOrWhiteSpace(password) || string.IsNullOrWhiteSpace(passwordHash))
                return false;

            return BCrypt.Net.BCrypt.Verify(password, passwordHash);
        }

        public async Task<Result<LoginDto>> RegisterUserAsync(
            string username,
            string email,
            string password,
            string? firstName = null,
            string? lastName = null)
        {
            if (await _userRepository.ExistsByUserNameAsync(username))
                return Result.Fail<LoginDto>("نام کاربری قبلاً ثبت شده است.");

            // اگر ایمیل هم unique باشد:
            // if (await _userRepository.ExistsByEmailAsync(email))
            //     return Result.Fail<LoginDto>("ایمیل قبلاً استفاده شده است.");

            var user = new User
            {
                Id = Guid.NewGuid(),
                UserName = username,
                NormalizedUserName = username.ToUpperInvariant(),
                Email = email,
                NormalizedEmail = email?.ToUpperInvariant(),
                PasswordHash = GeneratePasswordHash(password),
                FirstName = firstName,
                LastName = lastName,
                CreatedDate = DateTime.UtcNow,
                IsActive = true,
                IsDeleted = false,
                LoginAttempts = 0,
                IsLockedOut = false,
                ConcurrencyStamp = Guid.NewGuid().ToString()
            };

            try
            {
                await _userRepository.AddAsync(user);
                // await _userRepository.SaveChangesAsync();  ← اگر UnitOfWork دارید اینجا صدا بزنید

                return Result.OK(new LoginDto
                {
                    UserId = user.Id,
                    UserName = user.UserName,
                    Email = user.Email,
                    Message = "ثبت‌نام با موفقیت انجام شد."
                });
            }
            catch (Exception ex)
            {
                // لاگ بگیرید (مثلاً با ILogger)
                return Result.Fail<LoginDto>($"خطا در ثبت‌نام: {ex.Message}");
            }
        }

        public async Task<Result<LoginDto>> LoginUserAsync(string username, string password, Guid roleId)
        {
            var user = await _userRepository.GetByUserNameWithRolesAsync(username);
            if (user == null || user.IsDeleted)
                return Result.Fail<LoginDto>("نام کاربری یا رمز عبور اشتباه است.");

            if (!user.IsActive)
                return Result.Fail<LoginDto>("حساب کاربری غیرفعال است.");

            if (user.IsLockedOut && (user.LockoutEndDate == null || user.LockoutEndDate > DateTime.UtcNow))
                return Result.Fail<LoginDto>("حساب شما قفل شده است.");

            if (!user.UserRoles.Any(ur => ur.RoleId == roleId))
                return Result.Fail<LoginDto>($"شما به نقش درخواستی دسترسی ندارید.");

            if (!VerifyPassword(password, user.PasswordHash))
            {
                user.LoginAttempts++;
                if (user.LoginAttempts >= 5)
                {
                    user.IsLockedOut = true;
                    user.LockoutEndDate = DateTime.UtcNow.AddMinutes(15);
                }

                await _userRepository.UpdateAsync(user);
                // await _userRepository.SaveChangesAsync();

                await LogLoginAttempt(user.Id, false, "رمز عبور اشتباه");

                return Result.Fail<LoginDto>("نام کاربری یا رمز عبور اشتباه است.");
            }

            // ورود موفق
            user.LoginAttempts = 0;
            user.LastLoginDate = DateTime.UtcNow;
            user.IsLockedOut = false;
            user.LockoutEndDate = null;

            var refreshToken = GenerateRefreshToken();
            user.RefreshToken = refreshToken;
            user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(30);

            await _userRepository.UpdateAsync(user);
            // await _userRepository.SaveChangesAsync();

            await LogLoginAttempt(user.Id, true, null);

            var token = GenerateJwtToken(user);

            return Result.OK(new LoginDto
            {
                IsSuccess = true,
                UserId = user.Id,
                UserName = user.UserName,
                Email = user.Email,
                Token = token,
                TokenExpiry = DateTime.UtcNow.AddMinutes(_jwtSettings.ExpiryMinutes),
                RefreshToken = refreshToken,
                RefreshTokenExpiry = (DateTime)user.RefreshTokenExpiryTime,
                RoleId = roleId,
                Message = "ورود موفق"
            });
        }

        public async Task<Result> LogoutUserAsync(Guid userId)
        {
            var user = await _userRepository.GetByIdAsync(userId);
            if (user == null)
                return Result.Fail("کاربر یافت نشد.");

            user.LastLogoutDate = DateTime.UtcNow;
            user.RefreshToken = null;
            user.RefreshTokenExpiryTime = null;

            await _userRepository.UpdateAsync(user);
            // await _userRepository.SaveChangesAsync();

            await LogLoginAttempt(userId, true, null, LoginEventType.Logout);

            return Result.OK();
        }

        public async Task<Result> ChangePasswordAsync(Guid userId, string currentPassword, string newPassword)
        {
            var user = await _userRepository.GetByIdAsync(userId);
            if (user == null || user.IsDeleted || !user.IsActive)
                return Result.Fail("کاربر معتبر نیست.");

            if (!VerifyPassword(currentPassword, user.PasswordHash))
                return Result.Fail("رمز عبور فعلی اشتباه است.");

            user.PasswordHash = GeneratePasswordHash(newPassword);
            await _userRepository.UpdateAsync(user);
            // await _userRepository.SaveChangesAsync();

            return Result.OK();
        }

        public async Task<Result> ResetPasswordAsync(Guid userId, string newPassword, string? resetToken = null)
        {
            var user = await _userRepository.GetByIdAsync(userId);
            if (user == null || user.IsDeleted)
                return Result.Fail("کاربر یافت نشد.");

            // اگر از resetToken استفاده می‌کنید، اینجا چک کنید
            // if (!string.IsNullOrEmpty(resetToken) && !ValidateResetToken(user, resetToken))
            //     return Result.Fail("توکن بازنشانی نامعتبر است.");

            user.PasswordHash = GeneratePasswordHash(newPassword);
            user.IsLockedOut = false;
            user.LockoutEndDate = null;
            user.LoginAttempts = 0;

            await _userRepository.UpdateAsync(user);
            // await _userRepository.SaveChangesAsync();

            return Result.OK();
        }

        // متد کمکی برای ثبت تاریخچه ورود
        private async Task LogLoginAttempt(Guid userId, bool isSuccessful, string? failureReason = null, LoginEventType? eventType = null)
        {
            var context = _httpContextAccessor.HttpContext;

            string ipAddress = context?.Connection?.RemoteIpAddress?.ToString()
                            ?? context?.Request?.Headers["X-Forwarded-For"].FirstOrDefault()?.Split(',').First().Trim()
                            ?? "Unknown";

            string deviceInfo = context?.Request?.Headers["User-Agent"].ToString() ?? "Unknown";

            var history = new LoginHistory
            {
                UserId = userId,
                EventTime = DateTime.UtcNow,
                EventType = eventType ?? (isSuccessful ? LoginEventType.LoginSuccess : LoginEventType.LoginFailure),
                IpAddress = ipAddress,
                DeviceInfo = deviceInfo,
                IsSuccessful = isSuccessful,
                FailureReason = failureReason
            };

            await _userRepository.AddLoginHistoryAsync(history);
            // await _userRepository.SaveChangesAsync();  ← اگر UnitOfWork دارید اینجا صدا بزنید
        }

        private string GenerateJwtToken(User user)
        {
            if (string.IsNullOrEmpty(_jwtSettings.Key))
                throw new InvalidOperationException("Jwt Key is not configured.");

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.UserName ?? string.Empty),
                new Claim(ClaimTypes.Email, user.Email ?? string.Empty)
            };

            foreach (var role in user.UserRoles.Select(ur => ur.Role))
            {
                claims.Add(new Claim(ClaimTypes.Role, role.Name));
            }

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Key));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _jwtSettings.Issuer,
                audience: _jwtSettings.Audience,
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(_jwtSettings.ExpiryMinutes),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        private static string GenerateRefreshToken()
        {
            var randomBytes = RandomNumberGenerator.GetBytes(64);
            return Convert.ToBase64String(randomBytes)
                .TrimEnd('=')
                .Replace('+', '-')
                .Replace('/', '_');
        }
    }
}