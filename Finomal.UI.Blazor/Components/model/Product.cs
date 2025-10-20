using System.ComponentModel.DataAnnotations;

namespace Finomal.UI.Blazor.Components.model
{
    public class Product
    {
        public int Id { get; set; }
        public bool IsService { get; set; }

        [Required(ErrorMessage = "انتخاب گروه الزامی است")]
        public int? CategoryId { get; set; }

        public int? SubCategoryId { get; set; } // Optional

        [Required(ErrorMessage = "وارد کردن نام الزامی است")]
        public string Name { get; set; } = string.Empty;

        public string? SerialNumber { get; set; }
        public string? Barcode1 { get; set; }
        public string? Barcode2 { get; set; }

        [Required(ErrorMessage = "انتخاب واحد الزامی است")]
        public int? UnitId { get; set; }

        [Required(ErrorMessage = "انتخاب انبار الزامی است")]
        public int? WarehouseId { get; set; }

        [Range(0, double.MaxValue, ErrorMessage = "قیمت خرید نمی‌تواند منفی باشد")]
        public decimal PurchasePrice { get; set; }

        [Range(0, double.MaxValue, ErrorMessage = "قیمت فروش نمی‌تواند منفی باشد")]
        public decimal SellingPrice { get; set; }

        [Range(0, 100, ErrorMessage = "مقدار مالیات باید بین ۰ تا ۱۰۰ باشد")]
        public decimal TaxRate { get; set; } // As a percentage

        public bool IsActive { get; set; } = true;

        // Navigation properties for easy display
        public Category? Category { get; set; }
        public UnitOfMeasure? Unit { get; set; }

        public decimal StockQuantity { get; set; }
    }
}
