using Finomal.Application.Users;
using Finomal.Application.Users.Dtos;
using Finomal.Domain.Users;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using static BCrypt.Net.BCrypt;

namespace Finomal.Infrastructure.Data.Users
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly IUserRepository _userRepository;
        private readonly JwtSettings _jwtSettings;
        private readonly SignInManager<User> _signInManager;
        public AuthenticationService(IUserRepository userRepository, IOptions<JwtSettings> jwtSettings, SignInManager<User> signInManager)
        {
            _userRepository = userRepository;
            _jwtSettings = jwtSettings.Value;
            _signInManager = signInManager;
        }

        /// <summary>
        /// Generates a salted hash for the given password.
        /// </summary>
        /// <param name="password">The plain-text password.</param>
        /// <returns>The hashed password.</returns>
        public string GeneratePasswordHash(string password)
        {
            var a = 1;

            return HashPassword(password);
        }

        /// <summary>
        /// Verifies a plain-text password against a hashed password.
        /// </summary>
        /// <param name="password">The plain-text password to verify.</param>
        /// <param name="passwordHash">The hashed password from the database.</param>
        /// <returns>True if the password matches, false otherwise.</returns>
        public bool VerifyPassword(string password, string passwordHash)
        {
            return Verify(password, passwordHash);
        }

        /// <summary>
        /// Registers a new user in the system.
        /// </summary>
        /// <returns>A LoginDto indicating success/failure and user details.</returns>
        public async Task<LoginDto> RegisterUserAsync(string UserName, string email, string password, string firstName = null, string lastName = null)
        {
            // 1. بررسی وجود نام کاربری یا ایمیل
            var existingUserByUserName = await _userRepository.GetUserByUserNameAsync(UserName);
            if (existingUserByUserName != null)
            {
                return new LoginDto { IsSuccess = false, Message = "نام کاربری از قبل وجود دارد." };
            }

            // اگر نیاز به بررسی ایمیل هم هست:
            // var existingUserByEmail = await _userRepository.GetUserByEmailAsync(email);
            // if (existingUserByEmail != null) { /* ... */ }

            // 2. هش کردن رمز عبور
            var passwordHash = GeneratePasswordHash(password);

            // 3. ایجاد شیء کاربر
            var newUser = new User
            {
                UserName = UserName,
                Email = email,
                PasswordHash = passwordHash,
                FirstName = firstName,
                LastName = lastName,
                CreatedDate = DateTime.Now,
                IsActive = true,
                IsDeleted = false,
                LoginAttempts = 0,
                IsLockedOut = false,
                // RefreshToken و RefreshTokenExpiryTime را می‌توان بعد از اولین ورود تنظیم کرد
            };

            // 4. ذخیره کاربر در دیتابیس
            try
            {
                await _userRepository.AddUserAsync(newUser);

                // 5. تخصیص نقش پیش‌فرض (اختیاری)
                // اگر نقش "DefaultUser" یا "RegisteredUser" دارید و میخواهید آن را به صورت پیش‌فرض اضافه کنید.
                // فرض می‌کنیم RoleId=1 برای نقش پیش‌فرض است.
                // await _userRepository.AddUserToRoleAsync(newUser.UserID.ToString(), 1);

                // 6. ایجاد توکن (اگر مسئولیت این سرویس باشد)
                // var token = _jwtService.GenerateToken(newUser);
                // var tokenExpiry = DateTime.UtcNow.AddHours(1); // مثال

                return new LoginDto
                {
                    IsSuccess = true,
                    UserId = newUser.Id,
                    UserName = newUser.UserName,
                    Email = newUser.Email,
                    Message = "ثبت نام با موفقیت انجام شد."
                    // Token = token,
                    // TokenExpiry = tokenExpiry
                };
            }
            catch (Exception ex)
            {
                // لاگ خطا
                return new LoginDto { IsSuccess = false, Message = $"خطا در ثبت نام: {ex.Message}" };
            }
        }

        /// <summary>
        /// Authenticates a user with the given UserName and password.
        /// </summary>
        /// <returns>A LoginDto indicating success/failure and user details.</returns>
        public async Task<LoginDto> LoginUserAsync(string UserName, string password, int role)
        {

            var user = await _userRepository.GetUserByUserNameWithRolesAsync(UserName); 

            // 1. بررسی وجود کاربر
            if (user == null || user.IsDeleted)
            {
                return new LoginDto { IsSuccess = false, Message = "نام کاربری یا رمز عبور اشتباه است." };
            }

            // 2. بررسی فعال بودن حساب
            if (!user.IsActive)
            {
                return new LoginDto { IsSuccess = false, Message = "حساب کاربری شما غیرفعال است. لطفا با پشتیبانی تماس بگیرید." };
            }

            // 3. بررسی قفل بودن حساب
            if (user.IsLockedOut && (user.LockoutEndDate == null || user.LockoutEndDate > DateTime.UtcNow))
            {
                return new LoginDto { IsSuccess = false, Message = "حساب کاربری شما قفل شده است. لطفاً بعداً امتحان کنید یا رمز عبور خود را بازنشانی کنید." };
            }

            // 4. اعتبارسنجی نقش درخواستی (جدید)
            if (!user.UserRoles.Any(ur => ur.Role.Id == role)) 
            {
                // کاربر نقش درخواستی را ندارد
                return new LoginDto { IsSuccess = false, Message = $"شما دسترسی به عنوان {role} ندارید." };
            }

            // 5. بررسی رمز عبور
            if (!VerifyPassword(password, user.PasswordHash))
            {
                // افزایش تعداد تلاش‌های ناموفق
                user.LoginAttempts++;
                if (user.LoginAttempts >= 5)
                {
                    user.IsLockedOut = true;
                    user.LockoutEndDate = DateTime.UtcNow.AddMinutes(15);
                }
                await _userRepository.UpdateUserAsync(user);

                // ثبت تاریخچه ورود ناموفق
                await _userRepository.AddLoginHistoryAsync(new LoginHistory
                {
                    UserID = user.Id,
                    EventTime = DateTime.Now,
                    EventType = LoginEventType.LoginFailure,
                    IpAddress = "N/A", // باید از HttpContext دریافت شود
                    DeviceInfo = "N/A", // باید از HttpContext دریافت شود
                    FailureReason = "رمز عبور اشتباه", // اگر نیاز به جزئیات بیشتر دارید
                    IsSuccessful = false
                });

                return new LoginDto { IsSuccess = false, Message = "نام کاربری یا رمز عبور اشتباه است." };
            }

            // 6. ورود موفق: ریست کردن تلاش‌ها، به‌روزرسانی تاریخ ورود
            user.LoginAttempts = 0;
            user.LastLoginDate = DateTime.Now;
            user.IsLockedOut = false; // اگر قبلاً قفل شده بود، اکنون باز می‌شود
            user.LockoutEndDate = null;

            // 6.1. تولید و تنظیم RefreshToken (جدید)
            var refreshToken = GenerateRefreshToken(); 
            var refreshTokenExpiry = DateTime.UtcNow.AddDays(30);
            user.RefreshToken = refreshToken;
            user.RefreshTokenExpiryTime = refreshTokenExpiry;

            await _userRepository.UpdateUserAsync(user);

            // 7. ثبت تاریخچه ورود موفق
            await _userRepository.AddLoginHistoryAsync(new LoginHistory
            {
                UserID = user.Id,
                EventTime = DateTime.Now,
                EventType = LoginEventType.LoginSuccess,
                IpAddress = "N/A",
                DeviceInfo = "N/A",
                FailureReason = "N/A",
                IsSuccessful = false
            });

            // 8. تولید توکن (می‌توانید Role را نیز در Claims توکن JWT قرار دهید)
            var token = GenerateJwtToken(user);
            var tokenExpiry = DateTime.UtcNow.AddMinutes(_jwtSettings.ExpiryMinutes);

            var result = await _signInManager.PasswordSignInAsync(user.UserName, password, false, false);


            return new LoginDto
            {
                IsSuccess = true,
                UserId = user.Id,
                UserName = user.UserName,
                Email = user.Email,
                Message = "ورود با موفقیت انجام شد.",
                Token = token,
                TokenExpiry = tokenExpiry,
                RefreshToken = refreshToken, 
                RefreshTokenExpiry = refreshTokenExpiry, 
                Role = role
            };
        }

        /// <summary>
        /// Handles user logout, typically by invalidating tokens or session data.
        /// </summary>
        /// <param name="userId">The ID of the user logging out.</param>
        public async Task LogoutUserAsync(int userId)
        {
            var user = await _userRepository.GetUserByIdAsync(userId);
            if (user != null)
            {
                user.LastLogoutDate = DateTime.Now;
                user.RefreshToken = null; 
                user.RefreshTokenExpiryTime = null;
                await _userRepository.UpdateUserAsync(user);


                await _userRepository.AddLoginHistoryAsync(new LoginHistory
                {
                    UserID = user.Id,
                    EventTime = DateTime.Now,
                    EventType = LoginEventType.Logout,
                    IpAddress = "N/A", // باید از HttpContext دریافت شود
                    DeviceInfo = "N/A", // باید از HttpContext دریافت شود
                });

                await _signInManager.SignOutAsync();
            }

        }

        /// <summary>
        /// Allows a user to change their password, given their current password.
        /// </summary>
        /// <returns>True if password was changed successfully, false otherwise.</returns>
        public async Task<bool> ChangePasswordAsync(int userId, string currentPassword, string newPassword)
        {
            var user = await _userRepository.GetUserByIdAsync(userId);
            if (user == null || user.IsDeleted || !user.IsActive || user.IsLockedOut)
            {
                return false; 
            }

            if (!VerifyPassword(currentPassword, user.PasswordHash))
            {
                return false; 
            }

            user.PasswordHash = GeneratePasswordHash(newPassword);
            await _userRepository.UpdateUserAsync(user);
            return true;
        }

        /// <summary>
        /// Resets a user's password (e.g., by an administrator or forgot password flow).
        /// </summary>
        /// <returns>True if password was reset successfully, false otherwise.</returns>
        public async Task<bool> ResetPasswordAsync(int userId, string newPassword)
        {
            var user = await _userRepository.GetUserByIdAsync(userId);
            if (user == null || user.IsDeleted)
            {
                return false; 
            }
            
            user.PasswordHash = GeneratePasswordHash(newPassword);
            user.IsLockedOut = false;
            user.LockoutEndDate = null;
            user.LoginAttempts = 0;
            await _userRepository.UpdateUserAsync(user);
            return true;
        }

        string GenerateJwtToken(User user)
        {
            var claims = new List<Claim>
            {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Name, user.UserName),
            new Claim(ClaimTypes.Email, user.Email ?? string.Empty)
            };

            if (user.UserRoles != null)
            {
                foreach (var userRole in user.UserRoles)
                {
                    claims.Add(new Claim(ClaimTypes.Role, userRole.Role.Name));
                }
            }

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Key));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var expires = DateTime.UtcNow.AddMinutes(_jwtSettings.ExpiryMinutes);

            var token = new JwtSecurityToken(
                issuer: _jwtSettings.Issuer,
                audience: _jwtSettings.Audience,
                claims: claims,
                expires: expires,
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        private string GenerateRefreshToken()
        {
            var randomBytes = new byte[64];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(randomBytes);
            }

            return Convert.ToBase64String(randomBytes).TrimEnd('=').Replace('+', '-').Replace('/', '_');
        }
    }
}
