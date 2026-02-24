using Finomal.Domain.Accounting;
using Finomal.Domain.Users;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace Finomal.Infrastructure.Data.Context
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.ConfigureWarnings(w =>
                w.Ignore(RelationalEventId.PendingModelChangesWarning));
        }

        public DbSet<User> Users { get; set; } = null!;
        public DbSet<Role> Roles { get; set; } = null!;
        public DbSet<UserRole> UserRoles { get; set; } = null!;
        public DbSet<LoginHistory> LoginHistories { get; set; } = null!;
        public DbSet<AccountingDocument> AccountingDocuments { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // تنظیمات قبلی (جدول‌ها، ایندکس‌ها و روابط) بدون تغییر
            modelBuilder.Entity<User>().ToTable("Users");
            modelBuilder.Entity<Role>().ToTable("Roles");
            modelBuilder.Entity<UserRole>().ToTable("UserRoles");
            modelBuilder.Entity<LoginHistory>().ToTable("LoginHistories");
            modelBuilder.Entity<AccountingDocument>().ToTable("AccountingDocuments");

            // User configuration
            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(u => u.Id);
                entity.HasIndex(u => u.NormalizedUserName).IsUnique();
                entity.HasIndex(u => u.NormalizedEmail);
                entity.Property(u => u.UserName).IsRequired().HasMaxLength(256);
                entity.Property(u => u.NormalizedUserName).IsRequired().HasMaxLength(256);
                entity.Property(u => u.ConcurrencyStamp).IsConcurrencyToken();
            });

            // Role configuration
            modelBuilder.Entity<Role>(entity =>
            {
                entity.HasKey(r => r.Id);
                entity.HasIndex(r => r.NormalizedName).IsUnique();
                entity.Property(r => r.Name).IsRequired().HasMaxLength(256);
                entity.Property(r => r.NormalizedName).IsRequired().HasMaxLength(256);
                entity.Property(r => r.ConcurrencyStamp).IsConcurrencyToken();
            });

            // UserRole configuration
            modelBuilder.Entity<UserRole>(entity =>
            {
                entity.HasKey(ur => new { ur.UserId, ur.RoleId });
                entity.HasOne(ur => ur.User).WithMany(u => u.UserRoles).HasForeignKey(ur => ur.UserId).OnDelete(DeleteBehavior.Cascade);
                entity.HasOne(ur => ur.Role).WithMany(r => r.UserRoles).HasForeignKey(ur => ur.RoleId).OnDelete(DeleteBehavior.Cascade);
            });

            // LoginHistory configuration
            modelBuilder.Entity<LoginHistory>(entity =>
            {
                entity.HasKey(lh => lh.Id);
                entity.HasOne(lh => lh.User).WithMany(u => u.LoginHistories).HasForeignKey(lh => lh.UserId).OnDelete(DeleteBehavior.Cascade);
                entity.HasIndex(lh => new { lh.UserId, lh.EventTime });
            });

            // ────────────────────────────────────────────────
            //               Seed Data - داده‌های اولیه
            // ────────────────────────────────────────────────

            // 1. نقش پیش‌فرض: Administrator
            var adminRole = new Role
            {
                Id = Guid.NewGuid(),
                Name = "Administrator",
                NormalizedName = "ADMINISTRATOR",
                Description = "نقش مدیر سیستم با دسترسی کامل",
                CreatedDate = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc),
                ConcurrencyStamp = Guid.NewGuid().ToString()
            };

            modelBuilder.Entity<Role>().HasData(adminRole);

            // 2. کاربر پیش‌فرض: admin pass:Admin@1234
            var adminUser = new User
            {
                Id = Guid.NewGuid(),                    
                UserName = "admin",
                NormalizedUserName = "ADMIN",
                Email = "admin@finomal.ir",
                NormalizedEmail = "ADMIN@FINOMAL.IR",
                EmailConfirmed = true,
                PasswordHash = "$2a$2b$10$jwcaPIlNlCCBQe1EkgtENeugA6xTVJHikfZAirBrVSRnkQuHgoOSi", 
                SecurityStamp = Guid.NewGuid().ToString(),
                ConcurrencyStamp = Guid.NewGuid().ToString(),
                FirstName = "مدیر",
                LastName = "سیستم",
                CreatedDate = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc),
                IsActive = true,
                IsLockedOut = false,
                LockoutEnabled = true,
                AccessFailedCount = 0
            };

            modelBuilder.Entity<User>().HasData(adminUser);

            // 3. تخصیص نقش Administrator به کاربر admin
            modelBuilder.Entity<UserRole>().HasData(new UserRole
            {
                UserId = adminUser.Id,
                RoleId = adminRole.Id,
                AssignedDate = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc)
            });
        }
    }
}