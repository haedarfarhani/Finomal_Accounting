using System.ComponentModel.DataAnnotations;

namespace Finomal.UI.Blazor.Components.model
{
    public class WarehouseTransfer
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "شماره برگه انتقال الزامی است.")]
        public string TransferNumber { get; set; } = string.Empty;
        public DateTime TransferDate { get; set; } = DateTime.Now;

        [Required(ErrorMessage = "انتخاب انبار مبدا الزامی است.")]
        [Range(1, int.MaxValue, ErrorMessage = "انتخاب انبار مبدا الزامی است.")]
        public int SourceWarehouseId { get; set; }

        [Required(ErrorMessage = "انتخاب انبار مقصد الزامی است.")]
        [Range(1, int.MaxValue, ErrorMessage = "انتخاب انبار مقصد الزامی است.")]
        public int DestinationWarehouseId { get; set; }

        public string ShippingMethod { get; set; } = string.Empty; // نوع حمل
        public string DeliveredBy { get; set; } = string.Empty; // تحویل دهنده
        public string TrackingNumber { get; set; } = string.Empty; // شماره پیگیری
        public string Description { get; set; } = string.Empty; // شرح

        public bool IsFinalized { get; set; }
        public List<WarehouseTransferItem> Items { get; set; } = new();
    }

    public class WarehouseTransferItem
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public Product? Product { get; set; } // Navigation Property

        [Range(0.01, double.MaxValue, ErrorMessage = "مقدار باید بیشتر از صفر باشد.")]
        public decimal Quantity { get; set; }
    }
}
