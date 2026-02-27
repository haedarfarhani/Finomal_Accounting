namespace Finomal.Domain.Users
{
    public interface IUserRepository
    {
        // ────────────────────────────────────────────────
        //                 User - خواندن (Queries)
        // ────────────────────────────────────────────────

        Task<User?> GetByIdAsync(Guid userId);

        Task<User?> GetByUserNameAsync(string userName);

        Task<User?> GetByUserNameWithRolesAsync(string userName);

        Task<User?> GetByEmailAsync(string email);

        Task<bool> ExistsByUserNameAsync(string userName);

        Task<bool> ExistsByEmailAsync(string email);

        Task<IReadOnlyList<User>> GetAllAsync();

        Task<IReadOnlyList<User>> GetByRoleIdAsync(Guid roleId);

        // ────────────────────────────────────────────────
        //                 User - نوشتن (Commands)
        // ────────────────────────────────────────────────

        Task AddAsync(User user);

        Task UpdateAsync(User user);

        Task SoftDeleteAsync(Guid userId);

        Task RestoreAsync(Guid userId);

        // ────────────────────────────────────────────────
        //                    Roles
        // ────────────────────────────────────────────────

        Task<IReadOnlyList<Role>> GetRolesByUserIdAsync(Guid userId);

        Task<IReadOnlyList<Role>> GetAllRolesAsync();

        Task<Role?> GetRoleByIdAsync(Guid roleId);

        Task<Role?> GetRoleByNameAsync(string roleName);

        Task AddUserToRoleAsync(Guid userId, Guid roleId);

        Task RemoveUserFromRoleAsync(Guid userId, Guid roleId);

        Task<bool> IsUserInRoleAsync(Guid userId, Guid roleId);

        // ────────────────────────────────────────────────
        //                 Login History
        // ────────────────────────────────────────────────

        Task AddLoginHistoryAsync(LoginHistory loginHistory);

        Task<IReadOnlyList<LoginHistory>> GetLoginHistoryByUserIdAsync(
            Guid userId,
            DateTime? fromDate = null,
            DateTime? toDate = null,
            int? take = 50);

        // اگر نیاز به پاک کردن کل تاریخچه یا یک رکورد خاص دارید، می‌توانید اضافه کنید:
        // Task ClearLoginHistoryAsync(Guid userId);
        // Task DeleteLoginHistoryEntryAsync(int loginHistoryId);
    }
}