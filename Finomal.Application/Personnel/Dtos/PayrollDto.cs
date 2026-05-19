using System;

namespace Finomal.Application.Personnel.Dtos
{
    public class PayrollDto
    {
        public Guid Id { get; set; }
        public Guid PersonnelId { get; set; }
        public string PersonnelName { get; set; } = string.Empty;
        public string PersonnelCode { get; set; } = string.Empty;
        
        public int Year { get; set; }
        public int Month { get; set; }

        public decimal BaseSalary { get; set; }
        public decimal TotalAllowances { get; set; }
        public decimal TotalDeductions { get; set; }
        public decimal NetPay { get; set; }
        
        public string Status { get; set; } = "Calculated";
        
        // Breakdown for slip
        public decimal HousingAllowance { get; set; }
        public decimal FoodAllowance { get; set; }
        public decimal ChildAllowance { get; set; }
        public decimal OvertimeAmount { get; set; }
        public decimal OtherAllowances { get; set; }
        public decimal InsuranceDeduction { get; set; }
        public decimal TaxDeduction { get; set; }
        public decimal OtherDeductions { get; set; }
        
        public int WorkDays { get; set; }
    }
}
