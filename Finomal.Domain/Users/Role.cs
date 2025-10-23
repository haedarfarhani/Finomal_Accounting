using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace Finomal.Domain.Users
{
    public class Role : IdentityRole<int>
    {

        [StringLength(200)]
        public string? Description { get; set; }

        public DateTime CreatedDate { get; set; } = DateTime.Now;

        // Navigation Properties
        public virtual ICollection<UserRole> UserRoles { get; set; } = new List<UserRole>();
    }
}