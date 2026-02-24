using Finomal.Domain.Users;

namespace Finomal.Application.Users
{
    public interface IUserService
    {
        Guid? GetCurrentUserId();

        Task<bool> IsCurrentUserInRoleAsync(Guid roleId);

        Task<IReadOnlyList<Role>> GetCurrentUserRolesAsync();

        Task<IReadOnlyList<Role>> GetAllRolesAsync();

        Task<User?> GetUserByIdAsync(Guid userId);

        Task<User?> GetUserByUserNameAsync(string username);

        Task<Role?> GetRoleByIdAsync(Guid roleId);

        Task AddUserToRoleAsync(Guid userId, Guid roleId);

        Task RemoveUserFromRoleAsync(Guid userId, Guid roleId);
    }
}