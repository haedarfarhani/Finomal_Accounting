using Finomal.Application.Accounting.Dtos;

namespace Finomal.Application.Accounting.Dtos
{
    /// <summary>
    /// DTO برای نمایش، ویرایش و بازگرداندن اطلاعات یک سند حسابداری کامل.
    /// </summary>
    public class AccountingDocumentDto
    {
        // --- اطلاعات پایه از BaseEntity ---
        public Guid DocumentId { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public bool IsDeleted { get; set; }
        public string? CreatedByUserId { get; set; }
        public bool IsFinalized { get; set; }

        // --- اطلاعات اختصاصی سند حسابداری ---
        public string Description { get; set; }
        public DateTime Date { get; set; }
        public string? AccountingCode { get; set; }

        // --- اطلاعات روابط ---
        public int BranchId { get; set; }
        public string? BranchName { get; set; } // برای نمایش نام شعبه در UI

        public int BankId { get; set; }
        public string? BankName { get; set; } // برای نمایش نام بانک در UI

        // --- مقادیر محاسباتی ---
        public decimal TotalDebit { get; set; }
        public decimal TotalCredit { get; set; }

        /// <summary>
        /// لیستی از ردیف‌های این سند.
        /// </summary>
        public List<DocumentItemDto> Items { get; set; } = new List<DocumentItemDto>();
    }
}