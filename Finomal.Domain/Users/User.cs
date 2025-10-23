using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace Finomal.Domain.Users
{
    public class User : IdentityUser<int>
    {

        [StringLength(50)]
        public string FirstName { get; set; } // نام کوچک کاربر

        [StringLength(50)]
        public string LastName { get; set; } // نام خانوادگی کاربر

        public DateTime CreatedDate { get; set; } = DateTime.Now; // تاریخ ایجاد حساب کاربری

        public DateTime? LastLoginDate { get; set; } // آخرین تاریخ و زمان ورود به سیستم
                                                     // علامت سوال (?) نشان‌دهنده Nullable بودن (می‌تواند خالی باشد)

        public DateTime? LastLogoutDate { get; set; } // آخرین تاریخ و زمان خروج از سیستم (اختیاری، بسته به نحوه پیاده‌سازی)

        public DateTime? LastActivityDate { get; set; } // آخرین تاریخ و زمان فعالیت کاربر در سیستم
                                                        // (مفید برای شناسایی کاربران فعال/غیرفعال)

        public int LoginAttempts { get; set; } = 0; // تعداد تلاش‌های ناموفق برای ورود به سیستم
                                                    // (برای قفل کردن حساب پس از چند بار تلاش ناموفق)

        public bool IsLockedOut { get; set; } = true; // آیا حساب کاربری قفل شده است؟

        public DateTime? LockoutEndDate { get; set; } // تاریخ و زمان پایان قفل شدن حساب کاربری

        public bool IsActive { get; set; } = false; // آیا حساب کاربری فعال است؟

        public bool IsDeleted { get; set; } = false; // آیا حساب کاربری به صورت منطقی حذف شده است؟
                                                     // (به جای حذف فیزیکی از دیتابیس)

        [StringLength(255)]
        public string? RefreshToken { get; set; } // توکن رفرش برای سیستم‌های احراز هویت توکن-محور

        public DateTime? RefreshTokenExpiryTime { get; set; } // تاریخ انقضای توکن رفرش

        // Navigation Properties
        public virtual ICollection<UserRole> UserRoles { get; set; } = new List<UserRole>();

        // ممکن است نیاز به جداول ناوبری دیگری هم باشد، مانند:
        public virtual ICollection<LoginHistory> LoginHistories { get; set; }
        // (اگر بخواهید تاریخچه کامل ورود و خروج را نگهداری کنید)
    }
}