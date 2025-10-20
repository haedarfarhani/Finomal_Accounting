using System.ComponentModel.DataAnnotations;

namespace Finomal.UI.Blazor.Components.model
{


    public class EmploymentContract
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "انتخاب پرسنل الزامی است.")]
        [Range(1, int.MaxValue, ErrorMessage = "انتخاب پرسنل الزامی است.")]
        public int PersonId { get; set; }
        public Person? Person { get; set; } // Navigation property for display

        // --- Date Fields ---
        public DateTime? IssueDate { get; set; } = DateTime.Now; // تاریخ صدور
        [Required(ErrorMessage = "تاریخ اعتبار (شروع) الزامی است.")]
        public DateTime? EffectiveDate { get; set; } // تاریخ اعتبار
        public DateTime? TerminationDate { get; set; } // تاریخ پایان کار

        [Required(ErrorMessage = "تاریخ استخدام الزامی است.")]
        public DateTime? HireDate { get; set; }

        public string ContractType { get; set; } = "قراردادی"; // پیمانی، قراردادی، ...

        [Required(ErrorMessage = "انتخاب محل خدمت الزامی است.")]
        public int? WorkLocationId { get; set; }
        public WorkLocation? WorkLocation { get; set; }

        [Required(ErrorMessage = "انتخاب شغل الزامی است.")]
        public string JobTitle { get; set; } = "کارمند"; // کارمند، کارشناس، ...

        // --- بخش حقوق و مزایا ---
        [Range(0, double.MaxValue)]
        public decimal BaseSalary { get; set; }
        [Range(0, double.MaxValue)]
        public decimal ChildAllowance { get; set; }
        [Range(0, double.MaxValue)]
        public decimal HousingAllowance { get; set; }
        [Range(0, double.MaxValue)]
        public decimal FoodAllowance { get; set; }

        // --- بخش کسورات ---
        [Range(0, double.MaxValue)]
        public decimal SocialSecurityDeduction { get; set; } // کسر بیمه
        [Range(0, double.MaxValue)]
        public decimal SupplementalInsuranceDeduction { get; set; } // کسر بیمه تکمیلی
        [Range(0, double.MaxValue)]
        public decimal TaxDeduction { get; set; }

        // --- Computed Properties for instant feedback ---
        public decimal GrossPay => BaseSalary + ChildAllowance + HousingAllowance + FoodAllowance;
        public decimal TotalDeductions => SocialSecurityDeduction + SupplementalInsuranceDeduction + TaxDeduction;
        public decimal NetPay => GrossPay - TotalDeductions;
        public bool IsActive
        {
            get
            {
                var today = DateTime.Today;
                // A contract is active if today is on or after the effective date
                // AND there is no termination date OR today is before the termination date.
                return EffectiveDate.HasValue && today >= EffectiveDate.Value &&
                       (!TerminationDate.HasValue || today < TerminationDate.Value);
            }
        }
    }

    // A simple model for WorkLocation
    public class WorkLocation
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
    }
    public class CostCenter
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
    }
}
