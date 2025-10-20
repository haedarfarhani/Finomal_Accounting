using Finomal.Application.Abstractions;
using Finomal.Application.Accounting.Dtos;
using Finomal.Domain.Accounting;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Finomal.Application.Accounting.CreateDocument
{
    /// <summary>
    /// دستوری برای ایجاد یک سند حسابداری جدید.
    /// این کلاس داده‌های ورودی برای عملیات ایجاد را حمل می‌کند.
    /// </summary>
    public class CreateDocumentCommand : ICommand<AccountingDocumentDto>
    {
        [Required(ErrorMessage = "شرح سند الزامی است")]
        [MaxLength(500)]
        public string Description { get; set; }

        [Required]
        public DateTime Date { get; set; }

        [MaxLength(50)]
        public string? AccountingCode { get; set; } // اگر سیستم آن را تولید می‌کند، می‌تواند نال باشد

        [Required]
        public int BranchId { get; set; }

        [Required]
        public int BankId { get; set; }

        /// <summary>
        /// لیست آیتم‌های سند که باید همراه با آن ایجاد شوند.
        /// </summary>
        public List<DocumentItemDto> Items { get; set; } = new List<DocumentItemDto>();
    }
}