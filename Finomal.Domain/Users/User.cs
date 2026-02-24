using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity; // فقط برای استفاده از IPasswordHasher اگر نیاز داری

namespace Finomal.Domain.Users
{
    public class User
    {
        // کلید اصلی (به جای IdentityUser<Guid> خودت تعریف می‌کنی)
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();

        // پراپرتی‌های اصلی که IdentityUser داشت
        [Required]
        [StringLength(256)]
        public string UserName { get; set; } = string.Empty;

        [Required]
        [StringLength(256)]
        public string NormalizedUserName { get; set; } = string.Empty; // برای جستجوی case-insensitive

        [StringLength(256)]
        public string? Email { get; set; }

        public string? NormalizedEmail { get; set; }

        public bool EmailConfirmed { get; set; } = false;

        public string? PasswordHash { get; set; } // هش پسورد (با IPasswordHasher تولید می‌شه)

        public string? SecurityStamp { get; set; } // برای invalidate کردن توکن‌ها

        public string? ConcurrencyStamp { get; set; } = Guid.NewGuid().ToString(); // برای concurrency

        public string? PhoneNumber { get; set; }

        public bool PhoneNumberConfirmed { get; set; } = false;

        public bool TwoFactorEnabled { get; set; } = false;

        public bool LockoutEnabled { get; set; } = true;

        public DateTimeOffset? LockoutEnd { get; set; }

        public int AccessFailedCount { get; set; } = 0;


        [StringLength(50)]
        public string? FirstName { get; set; }

        [StringLength(50)]
        public string? LastName { get; set; }

        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;

        public DateTime? LastLoginDate { get; set; }

        public DateTime? LastLogoutDate { get; set; }

        public DateTime? LastActivityDate { get; set; }

        public int LoginAttempts { get; set; } = 0;

        public bool IsLockedOut { get; set; } = false; // توجه: قبلاً true بود – بهتره false باشد

        public DateTime? LockoutEndDate { get; set; }

        public bool IsActive { get; set; } = true; // بهتره پیش‌فرض true باشد مگر اینکه بخوای غیرفعال باشد

        public bool IsDeleted { get; set; } = false;

        [StringLength(512)]
        public string? RefreshToken { get; set; }

        public DateTime? RefreshTokenExpiryTime { get; set; }

        // Navigation Properties
        public virtual ICollection<UserRole> UserRoles { get; set; } = new List<UserRole>();

        // اگر تاریخچه ورود می‌خوای
        public virtual ICollection<LoginHistory> LoginHistories { get; set; } = new List<LoginHistory>();
    }
}