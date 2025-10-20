namespace Finomal.UI.Blazor.Components.model
{
    public class SalaryCalculationResult
    {
        public int Id { get; set; }
        public int PersonId { get; set; }
        public Person? Person { get; set; }

        public decimal NetPay { get; set; } // خالص پرداختی

        // اطلاعات تکمیلی
        public string? WorkLocationName { get; set; }
        public string? CostCenterName { get; set; }
        public DateTime? AccountingDocDate { get; set; }
        public string? AccountingDocNumber { get; set; }
    }

    // 2. مدل نتیجه محاسبه عیدی
    public class EidCalculationResult
    {
        public int Id { get; set; }
        public int PersonId { get; set; }
        public Person? Person { get; set; }

        public int EidDays { get; set; } // روزهای عیدی
        public decimal NetEidAmount { get; set; } // خالص عیدی

        // اطلاعات تکمیلی
        public string? WorkLocationName { get; set; }
        public string? CostCenterName { get; set; }
        public DateTime? AccountingDocDate { get; set; }
        public string? AccountingDocNumber { get; set; }
    }

    // 3. مدل نتیجه محاسبه سنوات
    public class SeveranceCalculationResult
    {
        public int Id { get; set; }
        public int PersonId { get; set; }
        public Person? Person { get; set; }

        public DateTime BasisDate { get; set; } // تاریخ مبنا
        public int SeveranceDays { get; set; } // روزهای سنوات
        public int DaysInYear { get; set; } // روز در سال سنوات

        // اطلاعات تکمیلی
        public string? WorkLocationName { get; set; }
        public string? CostCenterName { get; set; }
        public DateTime? AccountingDocDate { get; set; }
        public string? AccountingDocNumber { get; set; }
    }

}
