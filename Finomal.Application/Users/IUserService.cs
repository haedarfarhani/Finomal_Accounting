using Finomal.Domain.Users;

namespace Finomal.Application.Users
{
    public interface IUserService
    {
        Task<string> GetCurrentUserIdAsync();
        Task<bool> IsCurrentUserInRoleAsync(int roleId);
        Task<IReadOnlyList<Role>> GetUserRolesAsync(int userId);
        Task<IReadOnlyList<Role>> GetAllRolesAsync();
        Task AddUserToRoleAsync(int userId, int roleId);
        Task RemoveUserFromRoleAsync(int userId, int roleId);
        Task<User?> GetUserByUserNameAsync(string UserName);
        Task<User?> GetUserByIdAsync(int userId);
        Task<Role?> GetRoleByIdAsync(int roleId);
    }
}