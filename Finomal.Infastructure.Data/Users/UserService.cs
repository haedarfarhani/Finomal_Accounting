using Finomal.Application.Users;
using Finomal.Domain.Users;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Finomal.Infrastructure.Data.Users
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly string _jwtSecretKey;

        public UserService(
            IUserRepository userRepository,
            IHttpContextAccessor httpContextAccessor,
            IConfiguration configuration)
        {
            _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
            _httpContextAccessor = httpContextAccessor ?? throw new ArgumentNullException(nameof(httpContextAccessor));

            _jwtSecretKey = configuration["Jwt:Key"]
                ?? throw new InvalidOperationException("Jwt:Key در appsettings یافت نشد.");
        }

        /// <summary>
        /// استخراج شناسه کاربر (Guid) از توکن JWT در هدر Authorization
        /// </summary>
        public Guid? GetCurrentUserId()
        {
            var context = _httpContextAccessor.HttpContext;
            if (context == null)
            {
                return null;
            }

            string? authHeader = context.Request.Headers["Authorization"].FirstOrDefault();
            if (string.IsNullOrWhiteSpace(authHeader) || !authHeader.StartsWith("Bearer ", StringComparison.OrdinalIgnoreCase))
            {
                return null;
            }

            string token = authHeader["Bearer ".Length..].Trim();

            try
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var key = Encoding.UTF8.GetBytes(_jwtSecretKey);

                var validationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.Zero
                };

                var principal = tokenHandler.ValidateToken(token, validationParameters, out _);

                var userIdClaim = principal.FindFirst(ClaimTypes.NameIdentifier)?.Value
                               ?? principal.FindFirst("sub")?.Value;

                return Guid.TryParse(userIdClaim, out var userId) ? userId : null;
            }
            catch
            {
                // توکن نامعتبر یا هر خطای دیگر → کاربر شناسایی نشد
                return null;
            }
        }

        public async Task<bool> IsCurrentUserInRoleAsync(Guid roleId)
        {
            var userId = GetCurrentUserId();
            if (!userId.HasValue)
            {
                return false;
            }

            var roles = await GetCurrentUserRolesAsync();
            return roles.Any(r => r.Id == roleId);
        }

        public async Task<IReadOnlyList<Role>> GetCurrentUserRolesAsync()
        {
            var userId = GetCurrentUserId();
            if (!userId.HasValue)
            {
                return Array.Empty<Role>().AsReadOnly();
            }

            return await _userRepository.GetRolesByUserIdAsync(userId.Value);
        }

        public async Task<IReadOnlyList<Role>> GetAllRolesAsync()
        {
            return await _userRepository.GetAllRolesAsync();
        }

        public async Task<User?> GetUserByIdAsync(Guid userId)
        {
            return await _userRepository.GetByIdAsync(userId);
        }

        public async Task<User?> GetUserByUserNameAsync(string username)
        {
            if (string.IsNullOrWhiteSpace(username))
                return null;

            return await _userRepository.GetByUserNameAsync(username);
        }

        public async Task<Role?> GetRoleByIdAsync(Guid roleId)
        {
            return await _userRepository.GetRoleByIdAsync(roleId);
        }

        public async Task AddUserToRoleAsync(Guid userId, Guid roleId)
        {
            await _userRepository.AddUserToRoleAsync(userId, roleId);
        }

        public async Task RemoveUserFromRoleAsync(Guid userId, Guid roleId)
        {
            await _userRepository.RemoveUserFromRoleAsync(userId, roleId);
        }
    }
}