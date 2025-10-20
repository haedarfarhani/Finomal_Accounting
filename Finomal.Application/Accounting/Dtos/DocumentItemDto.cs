namespace Finomal.Application.Accounting.Dtos
{
    /// <summary>
    /// DTO برای نمایش و ویرایش یک ردیف (آیتم) از سند حسابداری در لایه کاربری.
    /// </summary>
    public class DocumentItemDto
    {
        /// <summary>
        /// شناسه آیتم در دیتابیس. برای آیتم‌های جدید، مقدار آن صفر است.
        /// </summary>
        public Guid ItemId { get; set; }

        /// <summary>
        /// شرح مربوط به این ردیف سند.
        /// </summary>
        public string? Description { get; set; }

        /// <summary>
        /// واحد پولی یا شمارشی (در صورت نیاز).
        /// </summary>
        public string? Unit { get; set; }

        /// <summary>
        /// مبلغ بدهکار.
        /// </summary>
        public decimal Debit { get; set; }

        /// <summary>
        /// مبلغ بستانکار.
        /// </summary>
        public decimal Credit { get; set; }

        /// <summary>
        /// شناسه حساب (BankAccount).
        /// </summary>
        public int AccountId { get; set; }

        /// <summary>
        /// شناسه تفصیل (Detailed).
        /// </summary>
        public int DetailId { get; set; }

        // --- می‌توانید فیلدهای نمایشی اضافی نیز برای راحتی در UI اضافه کنید ---

        /// <summary>
        /// عنوان حساب (برای نمایش در لیست‌های بازشونده).
        /// </summary>
        public string? AccountTitle { get; set; }
    }
}
