using Finomal.Domain.Abstractions;
using System.ComponentModel.DataAnnotations;

namespace Finomal.Domain.Accounting
{
    public class BankBranch : BaseEntity
    {
        [Required]
        [MaxLength(100)]
        public string Name { get; set; }
        public string Code { get; set; }
        public string? Address { get; set; }
    }
}