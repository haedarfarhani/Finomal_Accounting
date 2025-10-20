using Finomal.Domain.Abstractions;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Finomal.Domain.Accounting
{
    public class DocumentItem : BaseEntity
    {
        [MaxLength(500)]
        public string? Description { get; set; }

        [MaxLength(50)]
        public string? Unit { get; set; }

        public Guid ItemId { get; set; }

        // تعیین نوع داده دقیق برای مقادیر پولی در دیتابیس
        [Column(TypeName = "decimal(18, 2)")]
        public decimal RialAmount { get; set; }

        [Column(TypeName = "decimal(18, 2)")]
        public decimal DollarAmount { get; set; }

        [Column(TypeName = "decimal(18, 2)")]
        public decimal Debit { get; set; }

        [Column(TypeName = "decimal(18, 2)")]
        public decimal Credit { get; set; }

        // کلید خارجی برای سند حسابداری (ارتباط با جدول مادر)
        [Required]
        public int AccountingDocumentId { get; set; }
        [ForeignKey("AccountingDocumentId")]
        public virtual AccountingDocument AccountingDocument { get; set; }

        // کلید خارجی برای حساب
        [Required]
        public int AccountId { get; set; }
        [ForeignKey("AccountId")]
        public virtual BankAccount Account { get; set; }

        // کلید خارجی برای تفصیل (Detail)
        [Required]
        public int DetailId { get; set; }
        [ForeignKey("DetailId")]
        public virtual Detailed Detail { get; set; }
    }
}
