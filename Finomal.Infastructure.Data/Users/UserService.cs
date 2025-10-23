using Finomal.Application.Users;
using Finomal.Domain.Users;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace Finomal.Infrastructure.Data.Users
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public UserService(IUserRepository userRepository, IHttpContextAccessor httpContextAccessor)
        {
            _userRepository = userRepository;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<string> GetCurrentUserIdAsync()
        {
            //var principal = _httpContextAccessor.HttpContext?.User;
            //if (principal == null)
            //{
            //    throw new UnauthorizedAccessException("No authenticated user found.");
            //}

            //var userId = principal.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            //if (string.IsNullOrEmpty(userId))
            //{
            //    throw new UnauthorizedAccessException("User ID not found in claims.");
            //}

            return "userId";
        }

        public async Task<bool> IsCurrentUserInRoleAsync(int roleId)
        {
            var userId = await GetCurrentUserIdAsync();
            var roles = await GetUserRolesAsync(roleId);
            return roles.Any(rl => rl.Id == roleId);
        }

        public async Task<IReadOnlyList<Role>> GetUserRolesAsync(int userId)
        {
            return await _userRepository.GetUserRolesAsync(userId);
        }

        public async Task<IReadOnlyList<Role>> GetAllRolesAsync()
        {
            return await _userRepository.GetAllRolesAsync();
        }

        public async Task AddUserToRoleAsync(int userId, int roleId)
        {
            await _userRepository.AddUserToRoleAsync(userId, roleId);
        }

        public async Task RemoveUserFromRoleAsync(int userId, int roleId)
        {
            await _userRepository.RemoveUserFromRoleAsync(userId, roleId);
        }

        public async Task<User?> GetUserByUserNameAsync(string UserName)
        {
            return await _userRepository.GetUserByUserNameAsync(UserName);
        }

        public async Task<User?> GetUserByIdAsync(int userId)
        {
            return await _userRepository.GetUserByIdAsync(userId);
        }

        public Task<Role?> GetRoleByIdAsync(int roleId)
        {
            return _userRepository.GetRoleByIdAsync(roleId);
        }
    }
}