using E_CommerceSystem.Models;
using Microsoft.EntityFrameworkCore;

namespace E_CommerceSystem
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        // DbSets
        public DbSet<User> Users { get; set; } = default!; // Users table
        public DbSet<Product> Products { get; set; } = default!; // Products table
        public DbSet<Order> Orders { get; set; } = default!; // Orders table
        public DbSet<OrderProducts> OrderProducts { get; set; } = default!; // Junction table for Orders and Products
        public DbSet<Review> Reviews { get; set; } = default!; // Reviews table
        public DbSet<Category> Categories { get; set; } = default!; // Categories table
        public DbSet<Supplier> Suppliers { get; set; } = default!;
        public DbSet<RefreshToken> RefreshTokens { get; set; } = default!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // ===== قيود فريدة =====
            modelBuilder.Entity<User>()
                        .HasIndex(u => u.Email)
                        .IsUnique();

            modelBuilder.Entity<Category>()
                        .HasIndex(c => c.Name)
                        .IsUnique();

            modelBuilder.Entity<Supplier>()
                        .HasIndex(s => s.Name)
                        .IsUnique();

            // مراجعة واحدة لكل (User, Product)
            modelBuilder.Entity<Review>()
                        .HasIndex(r => new { r.PID, r.UID })
                        .IsUnique();

            // منع حذف متسلسل للفئة/المورّد
            modelBuilder.Entity<Product>()
                .HasOne(p => p.Category)
                .WithMany(c => c.Products)
                .HasForeignKey(p => p.CategoryId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Product>()
                .HasOne(p => p.Supplier)
                .WithMany(s => s.Products)
                .HasForeignKey(p => p.SupplierId)
                .OnDelete(DeleteBehavior.Restrict);

            // حفظ حالة الطلب كـ string بطول 20
            modelBuilder.Entity<Order>()
                .Property(o => o.Status)
                .HasConversion<string>()
                .HasMaxLength(20);

            // رموز توازي (Concurrency) بالتوقيت
            modelBuilder.Entity<Product>()
                .Property(p => p.RowVersion)
                .IsRowVersion();

            modelBuilder.Entity<Order>()
                .Property(o => o.RowVersion)
                .IsRowVersion();

            // قيود RefreshToken (منع التكرار لنفس المستخدم ونفس الهاش)
            modelBuilder.Entity<RefreshToken>()
                .HasIndex(rt => new { rt.UID, rt.TokenHash })
                .IsUnique();
        }
    }
}
