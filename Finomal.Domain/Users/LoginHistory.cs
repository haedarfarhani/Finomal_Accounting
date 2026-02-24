using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Finomal.Domain.Users
{
    /// <summary>
    /// انواع رویدادهای لاگین/خروج برای تاریخچه ورود کاربران
    /// </summary>
    public enum LoginEventType
    {
        LoginSuccess,
        LoginFailure,
        Logout,
        // AccountLocked,
        // TwoFactorRequired,
        // PasswordChanged
    }

    /// <summary>
    /// تاریخچه رویدادهای ورود، خروج و تلاش‌های ناموفق کاربر
    /// </summary>
    public class LoginHistory
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public Guid UserId { get; set; }

        [StringLength(45)]
        public string? IpAddress { get; set; }

        [StringLength(512)]
        public string? DeviceInfo { get; set; }

        public bool IsSuccessful { get; set; }

        [StringLength(255)]
        public string? FailureReason { get; set; }

        [Required]
        public DateTime EventTime { get; set; } = DateTime.UtcNow;

        public LoginEventType EventType { get; set; }

        [ForeignKey(nameof(UserId))]
        public virtual User User { get; set; } = null!;
    }
}