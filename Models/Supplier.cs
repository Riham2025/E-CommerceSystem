// Models/Supplier.cs
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using E_CommerceSystem.Models;

namespace E_CommerceSystem.Models
{
    public class Supplier
    {
        [Key] public int SupplierId { get; set; } // Primary Key

        [Required, MaxLength(120)] // Name of the supplier
        public string Name { get; set; } = null!; // Non-nullable

        [EmailAddress, MaxLength(200)] // Optional contact email
        public string? ContactEmail { get; set; } // Nullable

        [MaxLength(30)] // Optional phone number
        public string? Phone { get; set; }

        // 1 Supplier -> many Products
        public ICollection<Product> Products { get; set; } = new List<Product>();
    }
}

// جزء من Product (إن لم يكن موجوداً)
public int? CategoryId { get; set; }
public virtual Category? Category { get; set; }

public int? SupplierId { get; set; }
public virtual Supplier? Supplier { get; set; }
