using System;

namespace Finomal.Application.Personnel.Dtos
{
    public class PersonnelDto
    {
        public Guid Id { get; set; }

        // ── هویتی
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string FullName => $"{FirstName} {LastName}";
        public string FatherName { get; set; } = string.Empty;
        public string NationalCode { get; set; } = string.Empty;
        public string IdNumber { get; set; } = string.Empty;
        public string BirthDate { get; set; } = string.Empty;
        public string Gender { get; set; } = string.Empty;
        public string MaritalStatus { get; set; } = string.Empty;
        public int ChildrenCount { get; set; }
        public string Mobile { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;

        // ── شغلی
        public string PersonnelCode { get; set; } = string.Empty;
        public string HireDate { get; set; } = string.Empty;
        public string LeaveDate { get; set; } = string.Empty;
        public string EmploymentType { get; set; } = string.Empty;
        public string Department { get; set; } = string.Empty;
        public string JobTitle { get; set; } = string.Empty;
        public string CostCenter { get; set; } = string.Empty;
        public string WorkLocation { get; set; } = string.Empty;

        // ── مالی
        public string BankName { get; set; } = string.Empty;
        public string AccountNumber { get; set; } = string.Empty;
        public string Iban { get; set; } = string.Empty;
        public string AccountCodeCurrent { get; set; } = string.Empty;
        public string AccountCodeLoan { get; set; } = string.Empty;
        public string AccountCodeAdvance { get; set; } = string.Empty;

        public decimal BaseSalary { get; set; }
        public decimal HousingAllowance { get; set; }
        public decimal FoodAllowance { get; set; }
        public decimal ChildAllowance { get; set; }
        public decimal OvertimeFixed { get; set; }
        public decimal OtherAllowance { get; set; }

        // ── بیمه و مالیات
        public string InsuranceNumber { get; set; } = string.Empty;
        public string InsuranceType { get; set; } = string.Empty;
        public string EmployeeInsurancePct { get; set; } = string.Empty;
        public string EmployerInsurancePct { get; set; } = string.Empty;
        public bool UnemploymentInsurance { get; set; }
        public string TaxExemptionType { get; set; } = string.Empty;
        public string TaxServiceLocation { get; set; } = string.Empty;
        public string EmployerEconomicCode { get; set; } = string.Empty;
        public string EmployerNationalId { get; set; } = string.Empty;
        public bool IncomeTaxable { get; set; }
        public bool IncludeInInsuranceList { get; set; }

        // ── سیستمی
        public string Status { get; set; } = "active";
    }
}
