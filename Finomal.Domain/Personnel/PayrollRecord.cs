using System;
using System.ComponentModel.DataAnnotations;

namespace Finomal.Domain.Personnel
{
    public class PayrollRecord
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();

        public Guid PersonnelId { get; set; }
        public virtual Personnel Personnel { get; set; } = null!;

        public int Year { get; set; }
        public int Month { get; set; }

        public decimal BaseSalary { get; set; }
        public decimal HousingAllowance { get; set; }
        public decimal FoodAllowance { get; set; }
        public decimal ChildAllowance { get; set; }
        public decimal OvertimeAmount { get; set; }
        public decimal OtherAllowances { get; set; }

        public decimal InsuranceDeduction { get; set; }
        public decimal TaxDeduction { get; set; }
        public decimal OtherDeductions { get; set; }

        public decimal NetPay { get; set; }
        
        [StringLength(50)]
        public string Status { get; set; } = "Calculated"; // Calculated, Confirmed, Paid

        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
    }
}
