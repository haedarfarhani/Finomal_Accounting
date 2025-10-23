using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;

namespace Finomal.Domain.Users
{
    public class UserRole
    {
        [Key]
        public int UserRoleID { get; set; }

        [Required]
        public int UserID { get; set; }

        [Required]
        public int RoleID { get; set; }

        public DateTime AssignedDate { get; set; } = DateTime.Now;

        // Foreign Keys
        [ForeignKey("UserID")]
        public virtual User User { get; set; }

        [ForeignKey("RoleID")]
        public virtual Role Role { get; set; }
    }
}