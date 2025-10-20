using System.ComponentModel.DataAnnotations;

namespace Finomal.UI.Blazor.Components.model
{
    public class WarehouseReceipt
    {
        public int Id { get; set; }
        public string ReceiptNumber { get; set; } = string.Empty;
        public DateTime IssueDate { get; set; } = DateTime.Now;
        public string ReceiptType { get; set; } = "خرید"; // خرید، تولید، سایر
        public int WarehouseId { get; set; }

        public string SupplierName { get; set; } = string.Empty;
        public string SupplierAddress { get; set; } = string.Empty;
        public string SupplierPhone { get; set; } = string.Empty;

        public string ReceiverName { get; set; } = string.Empty;
        public string ReceiverAddress { get; set; } = string.Empty;
        public string ReceiverPhone { get; set; } = string.Empty;

        public decimal ShippingCost { get; set; }

        public bool IsFinalized { get; set; } = false; // وضعیت نهایی شدن رسید

        public List<WarehouseReceiptItem> Items { get; set; } = new List<WarehouseReceiptItem>();

        // Property for easy display
        public decimal TotalAmount => Items.Sum(i => i.Quantity * i.UnitPrice) + ShippingCost;
    }

    public class WarehouseReceiptItem
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public Product? Product { get; set; } // Navigation property to show product details

        [Range(0.01, double.MaxValue, ErrorMessage = "مقدار باید بیشتر از صفر باشد")]
        public decimal Quantity { get; set; }

        [Range(0, double.MaxValue, ErrorMessage = "فی کالا نمی‌تواند منفی باشد")]
        public decimal UnitPrice { get; set; }

        public string UnitName => Product?.Unit?.Name ?? "N/A";
    }
}
