using E_CommerceSystem.Models;
using Microsoft.EntityFrameworkCore;

namespace E_CommerceSystem
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        // DbSets
        public DbSet<User> Users { get; set; } = default!;
        public DbSet<Product> Products { get; set; } = default!;
        public DbSet<Order> Orders { get; set; } = default!;
        public DbSet<OrderProducts> OrderProducts { get; set; } = default!;
        public DbSet<Review> Reviews { get; set; } = default!;
        public DbSet<Category> Categories { get; set; } = default!;
        public DbSet<Supplier> Suppliers { get; set; } = default!;

        protected override void OnModelCreating(ModelBuilder modelBuilder) // Fluent API configurations
        {
            base.OnModelCreating(modelBuilder); // Call base method

            // Unique constraints
            modelBuilder.Entity<User>() 
                        .HasIndex(u => u.Email)
                        .IsUnique();

            // Unique constraint on Product Name within the same Supplier
            modelBuilder.Entity<Category>()
                        .HasIndex(c => c.Name)
                        .IsUnique();

            // Unique constraint on Supplier Name
            modelBuilder.Entity<Supplier>()
                        .HasIndex(s => s.Name)
                        .IsUnique();

            // Composite key for OrderProducts
            modelBuilder.Entity<Product>()
                .HasOne(p => p.Category)
                .WithMany(c => c.Products)
                .HasForeignKey(p => p.CategoryId)
                .OnDelete(DeleteBehavior.Restrict);

            // Prevent cascade delete to avoid accidental deletions
            modelBuilder.Entity<Product>()
                .HasOne(p => p.Supplier)
                .WithMany(s => s.Products)
                .HasForeignKey(p => p.SupplierId)
                .OnDelete(DeleteBehavior.Restrict);

            
            modelBuilder.Entity<Review>()
                .HasIndex(r => new { r.PID, r.UID })
                .IsUnique();

            // Enum to string conversion for Order Status
            modelBuilder.Entity<Order>()
                .Property(o => o.Status)
                .HasConversion<string>()
                .HasMaxLength(20);
            // Concurrency tokens
            modelBuilder.Entity<Product>()
                .Property(p => p.RowVersion)
                .IsRowVersion();
            // Concurrency token for Order
            modelBuilder.Entity<Order>()
                .Property(o => o.RowVersion)
                .IsRowVersion();




        }
    }
}

