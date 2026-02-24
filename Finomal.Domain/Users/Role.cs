using System.ComponentModel.DataAnnotations;

namespace Finomal.Domain.Users
{
    public class Role
    {

        [Key]
        public Guid Id { get; set; }

        [Required]
        [StringLength(256)]
        public string Name { get; set; } = string.Empty;

        [StringLength(256)]
        public string NormalizedName { get; set; } = string.Empty;   // برای جستجوی case-insensitive

        [StringLength(200)]
        public string? Description { get; set; }

        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;

        // Concurrency token (اختیاری ولی توصیه می‌شود)
        [ConcurrencyCheck]
        public string ConcurrencyStamp { get; set; } = Guid.NewGuid().ToString();

        // Navigation Properties
        public virtual ICollection<UserRole> UserRoles { get; set; } = new List<UserRole>();
    }
}