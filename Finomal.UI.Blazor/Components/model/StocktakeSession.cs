using static Finomal.UI.Blazor.Pages.CreateWarehouse;

namespace Finomal.UI.Blazor.Components.model
{
    // نشان‌دهنده یک دوره یا رویداد انبارگردانی
    public class StocktakeSession
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty; // مثال: "انبارگردانی پایان سال ۱۴۰۴"
        public DateTime StartDate { get; set; } = DateTime.Now;
        public int WarehouseId { get; set; }
        public Warehouse? Warehouse { get; set; }
        public bool IsCompleted { get; set; }
        public List<StocktakeItem> Items { get; set; } = new();
    }

    // نشان‌دهنده یک کالا در یک دوره شمارش
    public class StocktakeItem
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public Product? Product { get; set; }

        public decimal SystemQuantity { get; set; } // موجودی سیستمی در زمان شروع شمارش
        public decimal? CountedQuantity { get; set; } // مقدار شمارش شده (nullable چون ممکن است هنوز شمارش نشده باشد)

        // خصوصیت‌های محاسباتی برای گزارش‌ها
        public decimal Discrepancy => (CountedQuantity ?? 0) - SystemQuantity;
        public decimal DiscrepancyValue => Discrepancy * (Product?.SellingPrice ?? 0);
    }
}
