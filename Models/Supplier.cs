using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace E_CommerceSystem.Models
{
    public class Supplier
    {
        [Key] public int SupplierId { get; set; } // Primary Key

        [Required, MaxLength(120)] // Name of the supplier
        public string Name { get; set; } = null!; // Non-nullable

        [EmailAddress, MaxLength(200)] // Contact email of the supplier
        public string? ContactEmail { get; set; } // Nullable

        [Phone, MaxLength(30)] // Contact phone number of the supplier
        public string? Phone { get; set; } // Nullable

        // One Supplier → Many Products
        public ICollection<Product> Products { get; set; } = new List<Product>(); // Initialize to avoid null reference
    }
}

