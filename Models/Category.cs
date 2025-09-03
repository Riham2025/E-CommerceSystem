using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace E_CommerceSystem.Models
{
    public class Category
    {
        [Key] public int CategoryId { get; set; } // Primary Key

        [Required, MaxLength(100)] // Name of the category
        public string Name { get; set; } = null!; // Non-nullable

        [MaxLength(500)] // Optional description of the category
        public string? Description { get; set; } // Nullable

        // One Category → Many Products
        public ICollection<Product> Products { get; set; } = new List<Product>(); // Initialize to avoid null reference
    }
}
