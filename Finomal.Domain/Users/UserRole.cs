using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Finomal.Domain.Users;

namespace Finomal.Domain.Users
{
    public class UserRole
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }                     

        public Guid UserId { get; set; }
        public Guid RoleId { get; set; }

        public DateTime AssignedDate { get; set; } = DateTime.UtcNow;  

        // Navigation properties
        [ForeignKey(nameof(UserId))]
        public virtual User User { get; set; } = null!;

        [ForeignKey(nameof(RoleId))]
        public virtual Role Role { get; set; } = null!;

        // اگر نیاز به پراپرتی‌های اضافی داری (مثلاً AssignedByUserId, IsActive و ...)
        // public Guid? AssignedById { get; set; }
        // public bool IsActive { get; set; } = true;
    }
}