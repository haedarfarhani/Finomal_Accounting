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
        public DbSet<UserRole> userRoles { get; set; }
        public DbSet<LoginHistory> loginHistories { get; set; }
        public DbSet<AccountingDocument> AccountingDocuments { get; set; }
        public DbSet<Role> Roles { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // تعریف کلید اصلی ترکیبی برای UserRole
            modelBuilder.Entity<UserRole>()
                .HasKey(ur => new { ur.UserID, ur.RoleID });

            // تعریف ارتباط بین User و UserRole
            modelBuilder.Entity<UserRole>()
                .HasOne(ur => ur.User)
                .WithMany(u => u.UserRoles)
                .HasForeignKey(ur => ur.UserID);

            // تعریف ارتباط بین Role و UserRole
            modelBuilder.Entity<UserRole>()
                .HasOne(ur => ur.Role)
                .WithMany(r => r.UserRoles)
                .HasForeignKey(ur => ur.RoleID);

            // ارتباط User به LoginHistory
            modelBuilder.Entity<LoginHistory>()
                .HasOne(lh => lh.User)
                .WithMany(u => u.LoginHistories)
                .HasForeignKey(lh => lh.UserID);
        }
    }
}
