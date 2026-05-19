using System.ComponentModel.DataAnnotations;

namespace Finomal.Domain.Personnel
{
    public class PersonnelContract
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();

        public Guid PersonnelId { get; set; }
        public virtual Personnel Personnel { get; set; } = null!;

        [Required]
        [StringLength(100)]
        public string Title { get; set; } = string.Empty;

        [Required]
        [StringLength(50)]
        public string ContractNumber { get; set; } = string.Empty;

        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        public decimal MonthlySalary { get; set; }

        public bool IsActive { get; set; } = true;
        public bool IsDeleted { get; set; } = false;

        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
    }
}
