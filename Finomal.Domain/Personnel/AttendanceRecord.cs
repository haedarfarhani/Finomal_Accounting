using System;
using System.ComponentModel.DataAnnotations;

namespace Finomal.Domain.Personnel
{
    public class AttendanceRecord
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();

        public Guid PersonnelId { get; set; }
        public virtual Personnel Personnel { get; set; } = null!;

        public DateTime Date { get; set; }

        [StringLength(50)]
        public string Status { get; set; } = "work"; // work, absent, leave, overtime

        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
    }
}
