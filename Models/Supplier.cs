using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace E_CommerceSystem.Models
{
    public class Supplier
    {
        [Key] public int SupplierId { get; set; } // Primary Key

        [Required, MaxLength(120)] // Name of the supplier
        public string Name { get; set; } = null!;

        [EmailAddress, MaxLength(200)]
        public string? ContactEmail { get; set; }

        [Phone, MaxLength(30)]
        public string? Phone { get; set; }

        // One Supplier → Many Products
        public ICollection<Product> Products { get; set; } = new List<Product>();
    }
}

