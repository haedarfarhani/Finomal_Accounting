using Finomal.Domain.Abstractions;
using System.ComponentModel.DataAnnotations;

namespace Finomal.Domain.Accounting
{
    public class Bank : BaseEntity
    {
        [Required]
        [MaxLength(100)]
        public string Name { get; set; }
    }
}