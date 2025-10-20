using System.ComponentModel.DataAnnotations;
using static Finomal.UI.Blazor.Pages.CreateWarehouse;

namespace Finomal.UI.Blazor.Components.model
{
    public class WarehouseIssueVoucher
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "شماره حواله الزامی است")]
        public string VoucherNumber { get; set; } = string.Empty;

        public DateTime IssueDate { get; set; } = DateTime.Now;

        [Required(ErrorMessage = "انتخاب انبار الزامی است")]
        [Range(1, int.MaxValue, ErrorMessage = "انتخاب انبار الزامی است")]
        public int WarehouseId { get; set; }

        [Required(ErrorMessage = "نام تحویل گیرنده الزامی است")]
        public string IssuedTo { get; set; } = string.Empty; // تحویل گیرنده

        public bool IsBasedOnInvoice { get; set; }
        public string? AssociatedInvoiceNumber { get; set; }

        // For storing file info, not the file itself in the DB directly
        public string? UploadedInvoiceFileName { get; set; }

        public bool IsFinalized { get; set; }

        public List<WarehouseIssueVoucherItem> Items { get; set; } = new();

        // Navigation Properties
        public Warehouse? Warehouse { get; set; }
    }

    public class WarehouseIssueVoucherItem
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public Product? Product { get; set; }

        [Range(0.01, double.MaxValue, ErrorMessage = "مقدار باید بیشتر از صفر باشد")]
        public decimal Quantity { get; set; }
    }
}
