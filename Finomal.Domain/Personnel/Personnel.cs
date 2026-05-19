using System;
using System.ComponentModel.DataAnnotations;

namespace Finomal.Domain.Personnel
{
    public class Personnel
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();

        // ── هویتی
        [Required]
        [StringLength(100)]
        public string FirstName { get; set; } = string.Empty;

        [Required]
        [StringLength(100)]
        public string LastName { get; set; } = string.Empty;

        [StringLength(100)]
        public string FatherName { get; set; } = string.Empty;

        [Required]
        [StringLength(50)]
        public string NationalCode { get; set; } = string.Empty;

        [StringLength(50)]
        public string IdNumber { get; set; } = string.Empty;

        [StringLength(50)]
        public string BirthDate { get; set; } = string.Empty; // To keep it simple from Vue string

        [StringLength(20)]
        public string Gender { get; set; } = string.Empty;

        [StringLength(20)]
        public string MaritalStatus { get; set; } = string.Empty;

        public int ChildrenCount { get; set; }

        [StringLength(20)]
        public string Mobile { get; set; } = string.Empty;

        [StringLength(20)]
        public string Phone { get; set; } = string.Empty;

        [StringLength(500)]
        public string Address { get; set; } = string.Empty;

        // ── شغلی
        [StringLength(50)]
        public string PersonnelCode { get; set; } = string.Empty;

        [StringLength(50)]
        public string HireDate { get; set; } = string.Empty;
        
        [StringLength(50)]
        public string LeaveDate { get; set; } = string.Empty;

        [StringLength(50)]
        public string EmploymentType { get; set; } = string.Empty;

        [StringLength(100)]
        public string Department { get; set; } = string.Empty;

        [StringLength(100)]
        public string JobTitle { get; set; } = string.Empty;

        [StringLength(100)]
        public string CostCenter { get; set; } = string.Empty;

        [StringLength(200)]
        public string WorkLocation { get; set; } = string.Empty;

        // ── مالی
        [StringLength(100)]
        public string BankName { get; set; } = string.Empty;

        [StringLength(50)]
        public string AccountNumber { get; set; } = string.Empty;

        [StringLength(50)]
        public string Iban { get; set; } = string.Empty;

        [StringLength(50)]
        public string AccountCodeCurrent { get; set; } = string.Empty;

        [StringLength(50)]
        public string AccountCodeLoan { get; set; } = string.Empty;

        [StringLength(50)]
        public string AccountCodeAdvance { get; set; } = string.Empty;

        public decimal BaseSalary { get; set; }
        public decimal HousingAllowance { get; set; }
        public decimal FoodAllowance { get; set; }
        public decimal ChildAllowance { get; set; }
        public decimal OvertimeFixed { get; set; }
        public decimal OtherAllowance { get; set; }

        // ── بیمه و مالیات
        [StringLength(50)]
        public string InsuranceNumber { get; set; } = string.Empty;

        [StringLength(100)]
        public string InsuranceType { get; set; } = string.Empty;

        [StringLength(20)]
        public string EmployeeInsurancePct { get; set; } = string.Empty;

        [StringLength(20)]
        public string EmployerInsurancePct { get; set; } = string.Empty;

        public bool UnemploymentInsurance { get; set; }

        [StringLength(100)]
        public string TaxExemptionType { get; set; } = string.Empty;

        [StringLength(200)]
        public string TaxServiceLocation { get; set; } = string.Empty;

        [StringLength(50)]
        public string EmployerEconomicCode { get; set; } = string.Empty;

        [StringLength(50)]
        public string EmployerNationalId { get; set; } = string.Empty;

        public bool IncomeTaxable { get; set; }
        public bool IncludeInInsuranceList { get; set; }

        // ── سیستمی
        public string Status { get; set; } = "active"; // active, inactive
        public bool IsDeleted { get; set; } = false;
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
    }
}
