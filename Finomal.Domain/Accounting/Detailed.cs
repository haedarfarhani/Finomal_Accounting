using Finomal.Domain.Abstractions;
using System.ComponentModel.DataAnnotations;

namespace Finomal.Domain.Accounting
{
    //تفصیل حساب
    public class Detailed : BaseEntity
    {
        [Required]
        [MaxLength(150)]
        public string Name { get; set; }

        [Required]
        public int Code { get; set; }
    }
}