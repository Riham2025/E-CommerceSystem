using E_CommerceSystem.Models;
using Microsoft.EntityFrameworkCore;

namespace E_CommerceSystem
{
    public class ApplicationDbContext:DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; }

        public DbSet<Product> Products { get; set; }

        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderProducts> OrderProducts { get; set; }
        public DbSet<Review> Reviews { get; set; }

        public DbSet<Category> Categories => Set<Category>(); // Using expression-bodied property
        public DbSet<Supplier> Suppliers => Set<Supplier>(); // Using expression-bodied property


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            base.OnModelCreating(modelBuilder); // Call the base method

            modelBuilder.Entity<User>()
                        .HasIndex(u => u.Email)
                        .IsUnique();

            // Category (1) → (many) Products
            modelBuilder.Entity<Product>() 
                .HasOne(p => p.Category) // Each Product has one Category
                .WithMany(c => c.Products) // Each Category has many Products
                .HasForeignKey(p => p.CategoryId) // Foreign key in Product
                .OnDelete(DeleteBehavior.Restrict); // Prevent cascade delete


            // Supplier (1) → (many) Products
            modelBuilder.Entity<Product>()
                .HasOne(p => p.Supplier) // Each Product has one Supplier
                .WithMany(s => s.Products) // Each Supplier has many Products
                .HasForeignKey(p => p.SupplierId) // Foreign key in Product
                .OnDelete(DeleteBehavior.Restrict); // Prevent cascade delete

        }
    }
}
