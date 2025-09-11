using System.ComponentModel.DataAnnotations;

namespace E_CommerceSystem.Models
{
    public class SupplierDTO
    {
        [Required]
        public string Name { get; set; } = default!; // Non-nullable

        [EmailAddress]
        public string? ContactEmail { get; set; } // Nullable

        [Phone]
        public string? Phone { get; set; } // Nullable

        [Required]
        public int SupplierId { get; set; } // Primary Key

        // === Nested DTOs (as classes) for AutoMapper ===

        // Create
        public class SupplierCreateDto
        {
            [Required] // Non-nullable
            public string Name { get; set; } = default!;

            [EmailAddress] // Optional
            public string? ContactEmail { get; set; }

            [Phone] // Optional
            public string? Phone { get; set; } 
        }

        // Update
        public class SupplierUpdateDto
        {
            [Required] // Primary Key
            public string Name { get; set; } = default!;

            [EmailAddress] // Optional
            public string? ContactEmail { get; set; }

            [Phone] // Optional
            public string? Phone { get; set; }
        }

        // Read
        public class SupplierReadDto
        {
            [Required] // Primary Key
            public int SupplierId { get; set; }

            [Required] // Non-nullable
            public string Name { get; set; } = default!;

            [EmailAddress] // Optional
            public string? ContactEmail { get; set; }

            [Phone] // Optional
            public string? Phone { get; set; }
        }
    }
}
