using System;

namespace Finomal.Application.Personnel.Dtos
{
    public class PersonnelContractDto
    {
        public Guid Id { get; set; }
        public Guid PersonnelId { get; set; }
        public string PersonnelName { get; set; } = string.Empty;
        public string PersonnelInitials { get; set; } = string.Empty;
        public string JobTitle { get; set; } = string.Empty;
        
        public string Title { get; set; } = string.Empty; // Type of contract e.g. تمام وقت
        public string ContractNumber { get; set; } = string.Empty;
        
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string StartDateString { get; set; } = string.Empty;
        public string EndDateString { get; set; } = string.Empty;

        public decimal MonthlySalary { get; set; }

        public bool IsActive { get; set; }
        public bool IsExpiringSoon { get; set; }
        public string Status { get; set; } = string.Empty;
    }
}
