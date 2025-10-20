using System.ComponentModel.DataAnnotations;

namespace Finomal.UI.Blazor.Components.model
{
    public class MonthlyAttendanceRecord
    {
        public int Id { get; set; }
        public int PersonId { get; set; }
        public Person? Person { get; set; } // Navigation property

        public int Year { get; set; }
        public int Month { get; set; }

        // --- مقادیر قابل ویرایش ---
        [Range(0, double.MaxValue)]
        public decimal Bonus { get; set; } // پاداش نقدی

        [Range(0, double.MaxValue)]
        public decimal AdvancePayment { get; set; } // مساعده

        [Range(0, double.MaxValue)]
        public decimal OtherDeductions { get; set; } // سایر کسورات

        [Range(0, 31)]
        public decimal WorkingDays { get; set; } // کارکرد روزانه

        [Range(0, 31)]
        public decimal AbsenceDays { get; set; } // غیبت

        [Range(0, 1000)]
        public decimal OvertimeHours { get; set; } // کارکرد اضافه کاری (ساعت)

        [Range(0, 31)]
        public decimal MissionDays { get; set; } // کارکرد ماموریت روزانه

        [Range(0, 31)]
        public decimal LeaveDays { get; set; } // مرخصی

        public bool IsConfirmed { get; set; } = false; // وضعیت تایید نهایی
    }
}
