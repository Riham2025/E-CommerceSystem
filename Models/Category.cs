using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace E_CommerceSystem.Models
{
    public class Category
    {
        [Key] public int CategoryId { get; set; } // Primary Key

        [Required, MaxLength(100)] // Name of the category
        public string Name { get; set; } = null!;

        [MaxLength(500)]
        public string? Description { get; set; }

        // One Category → Many Products
        public ICollection<Product> Products { get; set; } = new List<Product>();
    }
}
