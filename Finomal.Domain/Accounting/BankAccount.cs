using Finomal.Domain.Abstractions;
using System.ComponentModel.DataAnnotations;

namespace Finomal.Domain.Accounting
{
    public class BankAccount : BaseEntity
    {
        [Required]
        [MaxLength(100)]
        public string Name { get; set; }
        public string Code { get; set; }
        public string PhoneNumber { get; set; }
    }
}