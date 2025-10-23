    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

namespace Finomal.Domain.Users 
{
    public enum LoginEventType // Enum برای نوع رویداد
    {
        LoginSuccess,
        LoginFailure,
        Logout
    }
    public class LoginHistory
    {
        [Key]
        public int LoginHistoryID { get; set; } // کلید اصلی جدول

        public int UserID { get; set; } // کلید خارجی برای ارتباط با جدول User

        [StringLength(50)]
        public string IpAddress { get; set; } // آدرس IP که کاربر از آن وارد شده است

        [StringLength(255)]
        public string DeviceInfo { get; set; } // اطلاعات دستگاه یا مرورگر کاربر (مثلاً User-Agent)

        public bool IsSuccessful { get; set; } // آیا این ورود موفق بوده است یا خیر؟
                                               // (مفید برای ردیابی تلاش‌های ناموفق ورود)

        [StringLength(255)]
        public string FailureReason { get; set; } // اگر ورود ناموفق بود، دلیل آن (مثلاً "رمز عبور اشتباه")

        [Required]
        public DateTime EventTime { get; set; } // زمان وقوع رویداد (ورود یا خروج)

        public LoginEventType EventType { get; set; } // نوع رویداد

        // Navigation Property
        // این خاصیت به Entity Framework Core کمک می‌کند تا ارتباط بین LoginHistory و User را درک کند.
        [ForeignKey("UserID")]
        public virtual User User { get; set; }
    }
}