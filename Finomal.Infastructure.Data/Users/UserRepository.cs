using Finomal.Domain.Users;
using Finomal.Infrastructure.Data.Context;
using Microsoft.EntityFrameworkCore;

namespace Finomal.Infrastructure.Data.Users
{
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationDbContext _context;

        public UserRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        // --- پیاده‌سازی متدهای بازیابی (Retrieval) ---

        public async Task<User?> GetUserByIdAsync(int userId)
        {
            return await _context.Users.Include(u => u.UserRoles)
                                     .ThenInclude(ur => ur.Role) // بارگذاری Role هم اگر نیاز دارید
                                 .FirstOrDefaultAsync(u => u.Id == userId);
        }

        public async Task<List<User>> GetUsersByIdsAsync(IEnumerable<int> userIds)
        {


            if (!userIds.Any()) return new List<User>();

            return await _context.Users
                                 .Include(u => u.UserRoles)
                                     .ThenInclude(ur => ur.Role)
                                 .Where(u => userIds.Contains(u.Id))
                                 .ToListAsync();
        }

        public async Task<User?> GetUserByUserNameAsync(string UserName)
        {
            return await _context.Users
                                 .Include(u => u.UserRoles)
                                     .ThenInclude(ur => ur.Role)
                                 .FirstOrDefaultAsync(u => u.UserName.ToLower() == UserName.ToLower());
        }

        public async Task<List<User>> GetAllUsersAsync()
        {
            return await _context.Users
                                 .Include(u => u.UserRoles)
                                     .ThenInclude(ur => ur.Role)
                                 .Where(u => !u.IsDeleted) // فقط کاربران حذف نشده را برگردان
                                 .ToListAsync();
        }

        // --- پیاده‌سازی متدهای مدیریت (Management) ---

        public async Task AddUserAsync(User user)
        {
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateUserAsync(User user)
        {
            // Attach the user if it's not already tracked by the context
            _context.Entry(user).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task DeleteUserAsync(int userId)
        {
            
            var user = await _context.Users.FindAsync(userId);
            if (user != null)
            {
                user.IsDeleted = true; // حذف منطقی
                await _context.SaveChangesAsync();
            }
        }

        public async Task ActivateUserAsync(int userId)
        {
            
            var user = await _context.Users.FindAsync(userId);
            if (user != null)
            {
                user.IsActive = true;
                user.IsLockedOut = false; // وقتی فعال می‌شود، قفل نیز برداشته شود
                user.LockoutEndDate = null;
                await _context.SaveChangesAsync();
            }
        }

        public async Task DeactivateUserAsync(int userId)
        {
            
            var user = await _context.Users.FindAsync(userId);
            if (user != null)
            {
                user.IsActive = false;
                await _context.SaveChangesAsync();
            }
        }

        public async Task LockUserAsync(int userId, DateTime? lockoutEndDate = null)
        {
            
            var user = await _context.Users.FindAsync(userId);
            if (user != null)
            {
                user.IsLockedOut = true;
                user.LockoutEndDate = lockoutEndDate ?? DateTime.UtcNow.AddMinutes(30); // قفل پیش‌فرض 30 دقیقه
                user.LoginAttempts = 0; // تلاش‌های ناموفق ریست شود
                await _context.SaveChangesAsync();
            }
        }

        public async Task UnlockUserAsync(int userId)
        {
            
            var user = await _context.Users.FindAsync(userId);
            if (user != null)
            {
                user.IsLockedOut = false;
                user.LockoutEndDate = null;
                user.LoginAttempts = 0; // تلاش‌های ناموفق ریست شود
                await _context.SaveChangesAsync();
            }
        }

        // --- پیاده‌سازی متدهای مدیریت نقش‌ها (Role Management) ---

        public async Task AddUserToRoleAsync(int userId, int roleId)
        {
            
            var user = await _context.Users
                                 .Include(u => u.UserRoles)
                                 .FirstOrDefaultAsync(u => u.Id == userId);

            if (user != null && !user.UserRoles.Any(ur => ur.RoleID == roleId))
            {
                user.UserRoles.Add(new UserRole { UserID = userId, RoleID = roleId });
                await _context.SaveChangesAsync();
            }
        }

        public async Task RemoveUserFromRoleAsync(int userId, int roleId)
        {
            
            var userRole = await _context.userRoles // فرض می‌کنیم UserRoles یک DbSet در DbContext است
                                        .FirstOrDefaultAsync(ur => ur.UserID == userId && ur.RoleID == roleId);

            if (userRole != null)
            {
                _context.userRoles.Remove(userRole);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<List<Role>> GetUserRolesAsync(int userId)
        {
            
            return await _context.userRoles // فرض می‌کنیم UserRoles یک DbSet در DbContext است
                                 .Where(ur => ur.UserID == userId)
                                 .Select(ur => ur.Role) // فرض می‌کنیم UserRole دارای یک Navigation Property به Role باشد
                                 .ToListAsync();
        }

        // --- پیاده‌سازی متدهای تاریخچه (History/Log) ---

        public async Task<List<LoginHistory>> GetUserLoginHistoryAsync(int userId, int count = 20)
        {

            return await _context.loginHistories // فرض می‌کنیم LoginHistories یک DbSet در DbContext است
                                 .Where(lh => lh.UserID == userId)
                                 .OrderByDescending(lh => lh.EventTime) // جدیدترین ورودها اول باشند
                                 .Take(count) // محدود کردن تعداد نتایج
                                 .ToListAsync();
        }

        public async Task AddLoginHistoryAsync(LoginHistory loginHistory)
        {
            _context.loginHistories.Add(loginHistory);
            await _context.SaveChangesAsync();
        }

        public async Task ClearUserLoginHistoryAsync(int userId)
        {
           

            var userLoginHistories = await _context.loginHistories
                                                   .Where(lh => lh.UserID == userId)
                                                   .ToListAsync();
            if (userLoginHistories.Any())
            {
                _context.loginHistories.RemoveRange(userLoginHistories);
                await _context.SaveChangesAsync();
            }
        }

        public async Task DeleteLoginHistoryEntryAsync(int loginHistoryId)
        {
            var loginHistoryEntry = await _context.loginHistories.FindAsync(loginHistoryId);
            if (loginHistoryEntry != null)
            {
                _context.loginHistories.Remove(loginHistoryEntry);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<User> GetUserByUserNameWithRolesAsync(string UserName)
        {
            var userWithRole = await _context.Users
                       .Include(u => u.UserRoles)
                           .ThenInclude(ur => ur.Role)
                       .FirstOrDefaultAsync(u => u.UserName == UserName);
            return userWithRole;

        }

        public async Task<IReadOnlyList<Role>> GetAllRolesAsync()
        {
            return await _context.Roles.ToListAsync();
        }

        public async Task<Role> GetRoleByIdAsync(int roleId)
        {
            return await _context.Roles.FirstOrDefaultAsync(r => r.Id == roleId);
        }
    }
}
