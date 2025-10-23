namespace Finomal.Domain.Users
{
    public interface IUserRepository
    {
        // --- متدهای بازیابی (Retrieval) ---
        Task<User?> GetUserByIdAsync(int userId); // بازیابی کاربر بر اساس ID (string)
        Task<List<User>> GetUsersByIdsAsync(IEnumerable<int> userIds); // بازیابی لیستی از کاربران بر اساس IDها (string)
        Task<User?> GetUserByUserNameAsync(string UserName); // بازیابی کاربر بر اساس نام کاربری

        Task<List<User>> GetAllUsersAsync(); // بازیابی تمامی کاربران

        // --- متدهای مدیریت (Management) ---
        Task AddUserAsync(User user); // اضافه کردن کاربر جدید
        Task UpdateUserAsync(User user); // بروزرسانی کاربر موجود
        Task DeleteUserAsync(int userId); // حذف منطقی کاربر بر اساس ID (string)

        Task ActivateUserAsync(int userId); // فعال کردن کاربر
        Task DeactivateUserAsync(int userId); // غیرفعال کردن کاربر
        Task LockUserAsync(int userId, DateTime? lockoutEndDate = null); // قفل کردن کاربر
        Task UnlockUserAsync(int userId); // باز کردن قفل کاربر

        // --- متدهای مدیریت نقش‌ها (Role Management) ---
        Task AddUserToRoleAsync(int userId, int roleId); // اضافه کردن کاربر به یک نقش
        Task RemoveUserFromRoleAsync(int userId, int roleId); // حذف کاربر از یک نقش
        Task<List<Role>> GetUserRolesAsync(int userId); // گرفتن لیست نقش‌های یک کاربر
        Task<IReadOnlyList<Role>> GetAllRolesAsync(); // گرفتن لیست نقش‌های یک کاربر
        Task<User> GetUserByUserNameWithRolesAsync(string UserName);
        Task<Role> GetRoleByIdAsync(int userId);
        // --- متدهای تاریخچه (History/Log) ---
        Task AddLoginHistoryAsync(LoginHistory loginHistory); // اضافه کردن یک رکورد تاریخچه ورود جدید
        Task ClearUserLoginHistoryAsync(int userId); // پاک کردن تمام تاریخچه ورود یک کاربر
        Task DeleteLoginHistoryEntryAsync(int loginHistoryId); // پاک کردن یک رکورد خاص از تاریخچه ورود
    }
}