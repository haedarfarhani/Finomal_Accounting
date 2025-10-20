using System.ComponentModel.DataAnnotations;

namespace Finomal.UI.Blazor.Components.model
{
    public class Person
    {
        public int Id { get; set; }
        public string PersonnelCode { get; set; } = string.Empty;
        public PersonIdentityInfo Identity { get; set; } = new();
        public PersonContactInfo Contact { get; set; } = new();
        public PersonComplementaryInfo Complementary { get; set; } = new();
        public PersonSocialSecurityInfo SocialSecurity { get; set; } = new();
        public PersonSupplementalInsuranceInfo SupplementalInsurance { get; set; } = new();
        public PersonAccountInfo Account { get; set; } = new();
    }

    public class PersonIdentityInfo
    {
        public string? ProfilePictureUrl { get; set; } // Base64 or URL
        [Required(ErrorMessage = "نام الزامی است")]
        public string FirstName { get; set; } = string.Empty;
        [Required(ErrorMessage = "نام خانوادگی الزامی است")]
        public string LastName { get; set; } = string.Empty;
        public string FatherName { get; set; } = string.Empty;
        public string BirthPlace { get; set; } = string.Empty;
        public string IssuePlace { get; set; } = string.Empty;
        public DateTime? DateOfBirth { get; set; }
        [Required(ErrorMessage = "کد ملی الزامی است")]
        public string NationalId { get; set; } = string.Empty;
        public string CertificateNumber { get; set; } = string.Empty;
        public string Gender { get; set; } = "مرد";
        public string Nationality { get; set; } = "ایرانی";
    }

    public class PersonContactInfo
    {
        public string Address { get; set; } = string.Empty;
        public string PostalCode { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        [EmailAddress(ErrorMessage = "فرمت ایمیل صحیح نیست")]
        public string Email { get; set; } = string.Empty;
    }

    public class PersonComplementaryInfo
    {
        public string Status { get; set; } = "فعال"; // فعال، غیرفعال، ...
        public string MaritalStatus { get; set; } = "مجرد";
        [Range(0, 20)]
        public int ChildrenCount { get; set; }
        public string MilitaryServiceStatus { get; set; } = string.Empty;
        public bool IsVeteran { get; set; }
        public string EducationLevel { get; set; } = string.Empty;
        public string FieldOfStudy { get; set; } = string.Empty;
        public string OrganizationalUnit { get; set; } = string.Empty;
        public string WorkExperience { get; set; } = string.Empty;
    }

    public class PersonSocialSecurityInfo
    {
        public string WorkshopCode { get; set; } = string.Empty;
        public string InsuranceType { get; set; } = string.Empty;
        public string InsuranceNumber { get; set; } = string.Empty;
        public string InsuranceJobCode { get; set; } = string.Empty;
        public DateTime? InsuranceStartDate { get; set; }
    }

    public class PersonSupplementalInsuranceInfo
    {
        public string InsuranceType { get; set; } = "ندارد";
    }

    public class PersonAccountInfo
    {
        public string BankName { get; set; } = string.Empty;
        public string BranchName { get; set; } = string.Empty;
        public string AccountNumber { get; set; } = string.Empty;
        public string Iban { get; set; } = string.Empty; // شماره شبا
        public string PaymentFrequency { get; set; } = "ماهیانه"; // ماهیانه، ساعتی، ...
    }
}
