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
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        // ────────────────────────────────────────────────
        //                 User - خواندن (Queries)
        // ────────────────────────────────────────────────

        public async Task<User?> GetByIdAsync(Guid userId)
        {
            return await _context.Users
                .Include(u => u.UserRoles)
                    .ThenInclude(ur => ur.Role)
                .FirstOrDefaultAsync(u => u.Id == userId && !u.IsDeleted);
        }

        public async Task<User?> GetByUserNameAsync(string userName)
        {
            if (string.IsNullOrWhiteSpace(userName))
                return null;

            return await _context.Users
                .Include(u => u.UserRoles)
                    .ThenInclude(ur => ur.Role)
                .FirstOrDefaultAsync(u => u.NormalizedUserName == userName.ToUpperInvariant() && !u.IsDeleted);
        }

        public async Task<User?> GetByUserNameWithRolesAsync(string userName)
        {
            return await GetByUserNameAsync(userName); // همین متد قبلی کافیه
        }

        public async Task<User?> GetByEmailAsync(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                return null;

            return await _context.Users
                .Include(u => u.UserRoles)
                    .ThenInclude(ur => ur.Role)
                .FirstOrDefaultAsync(u => u.NormalizedEmail == email.ToUpperInvariant() && !u.IsDeleted);
        }

        public async Task<bool> ExistsByUserNameAsync(string userName)
        {
            if (string.IsNullOrWhiteSpace(userName))
                return false;

            return await _context.Users
                .AnyAsync(u => u.NormalizedUserName == userName.ToUpperInvariant() && !u.IsDeleted);
        }

        public async Task<bool> ExistsByEmailAsync(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                return false;

            return await _context.Users
                .AnyAsync(u => u.NormalizedEmail == email.ToUpperInvariant() && !u.IsDeleted);
        }

        public async Task<IReadOnlyList<User>> GetAllAsync()
        {
            return await _context.Users
                .Include(u => u.UserRoles)
                    .ThenInclude(ur => ur.Role)
                .Where(u => !u.IsDeleted)
                .ToListAsync();
        }

        public async Task<IReadOnlyList<User>> GetByRoleIdAsync(Guid roleId)
        {
            return await _context.Users
                .Include(u => u.UserRoles)
                    .ThenInclude(ur => ur.Role)
                .Where(u => u.UserRoles.Any(ur => ur.RoleId == roleId) && !u.IsDeleted)
                .ToListAsync();
        }

        // ────────────────────────────────────────────────
        //                 User - نوشتن (Commands)
        // ────────────────────────────────────────────────

        public async Task AddAsync(User user)
        {
            if (user == null)
                throw new ArgumentNullException(nameof(user));

            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(User user)
        {
            if (user == null)
                throw new ArgumentNullException(nameof(user));

            _context.Entry(user).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task SoftDeleteAsync(Guid userId)
        {
            var user = await _context.Users.FindAsync(userId);
            if (user == null || user.IsDeleted)
                return;

            user.IsDeleted = true;
            user.IsActive = false;
            await _context.SaveChangesAsync();
        }

        public async Task RestoreAsync(Guid userId)
        {
            var user = await _context.Users.FindAsync(userId);
            if (user == null || !user.IsDeleted)
                return;

            user.IsDeleted = false;
            user.IsActive = true;
            await _context.SaveChangesAsync();
        }

        // ────────────────────────────────────────────────
        //                    Roles
        // ────────────────────────────────────────────────

        public async Task<IReadOnlyList<Role>> GetRolesByUserIdAsync(Guid userId)
        {
            return await _context.UserRoles
                .Where(ur => ur.UserId == userId)
                .Select(ur => ur.Role)
                .ToListAsync();
        }

        public async Task<IReadOnlyList<Role>> GetAllRolesAsync()
        {
            return await _context.Roles.ToListAsync();
        }

        public async Task<Role?> GetRoleByIdAsync(Guid roleId)
        {
            return await _context.Roles.FindAsync(roleId);
        }

        public async Task<Role?> GetRoleByNameAsync(string roleName)
        {
            if (string.IsNullOrWhiteSpace(roleName))
                return null;

            return await _context.Roles
                .FirstOrDefaultAsync(r => r.NormalizedName == roleName.ToUpperInvariant());
        }

        public async Task AddUserToRoleAsync(Guid userId, Guid roleId)
        {
            var exists = await _context.UserRoles
                .AnyAsync(ur => ur.UserId == userId && ur.RoleId == roleId);

            if (exists)
                return;

            var userRole = new UserRole
            {
                UserId = userId,
                RoleId = roleId,
                AssignedDate = DateTime.UtcNow
            };

            await _context.UserRoles.AddAsync(userRole);
            await _context.SaveChangesAsync();
        }

        public async Task RemoveUserFromRoleAsync(Guid userId, Guid roleId)
        {
            var userRole = await _context.UserRoles
                .FirstOrDefaultAsync(ur => ur.UserId == userId && ur.RoleId == roleId);

            if (userRole != null)
            {
                _context.UserRoles.Remove(userRole);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<bool> IsUserInRoleAsync(Guid userId, Guid roleId)
        {
            return await _context.UserRoles
                .AnyAsync(ur => ur.UserId == userId && ur.RoleId == roleId);
        }

        // ────────────────────────────────────────────────
        //                 Login History
        // ────────────────────────────────────────────────

        public async Task AddLoginHistoryAsync(LoginHistory loginHistory)
        {
            if (loginHistory == null)
                throw new ArgumentNullException(nameof(loginHistory));

            await _context.LoginHistories.AddAsync(loginHistory);
            await _context.SaveChangesAsync();
        }

        public async Task<IReadOnlyList<LoginHistory>> GetLoginHistoryByUserIdAsync(
            Guid userId,
            DateTime? fromDate = null,
            DateTime? toDate = null,
            int? take = 50)
        {
            var query = _context.LoginHistories
                .Where(lh => lh.UserId == userId)
                .OrderByDescending(lh => lh.EventTime);

            if (fromDate.HasValue)
                query = (IOrderedQueryable<LoginHistory>)query.Where(lh => lh.EventTime >= fromDate.Value);

            if (toDate.HasValue)
                query = (IOrderedQueryable<LoginHistory>)query.Where(lh => lh.EventTime <= toDate.Value);

            if (take.HasValue && take.Value > 0)
                query = (IOrderedQueryable<LoginHistory>)query.Take(take.Value);

            return await query.ToListAsync();
        }
    }
}