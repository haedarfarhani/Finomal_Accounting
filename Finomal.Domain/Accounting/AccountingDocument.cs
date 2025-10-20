using Finomal.Domain.Abstractions;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Finomal.Domain.Accounting
{
    public class AccountingDocument : BaseEntity
    {
        [Required(ErrorMessage = "شرح سند الزامی است")]
        [MaxLength(500)] // تعیین حداکثر طول برای بهینه‌سازی دیتابیس
        public string Description { get; set; }

        [Required]
        public DateTime Date { get; set; }

        [MaxLength(50)]
        public Guid? DocumentId { get; set; }

        public bool? IsFinalized { get; set; }

        [Column(TypeName = "decimal(18, 2)")]
        public decimal TotalDebit { get; set; }

        [Column(TypeName = "decimal(18, 2)")]
        public decimal TotalCredit { get; set; }

        // --- تعریف روابط ---

        // کلید خارجی برای شعبه
        [Required]
        public int BranchId { get; set; }
        // خاصیت ناوبری برای دسترسی به اطلاعات شعبه
        [ForeignKey("BranchId")]
        public virtual BankBranch Branch { get; set; }

        // کلید خارجی برای بانک
        [Required]
        public int BankId { get; set; }
        // خاصیت ناوبری برای دسترسی به اطلاعات بانک
        [ForeignKey("BankId")]
        public virtual Bank Bank { get; set; }

        // رابطه یک-به-چند با آیتم‌های سند
        // این نشان می‌دهد که هر سند می‌تواند چندین آیتم داشته باشد
        public virtual ICollection<DocumentItem> Items { get; set; } = new List<DocumentItem>();
    }
}
