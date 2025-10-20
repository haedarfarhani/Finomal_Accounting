using Finomal.Domain.Accounting;
using Finomal.Domain.Users;
using Microsoft.EntityFrameworkCore;

namespace Finomal.Infrastructure.Data.Context
{
    public class ApplicationDbContext:DbContext
    {
        public ApplicationDbContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<AccountingDocument> AccountingDocuments { get; set; }
    }
}
