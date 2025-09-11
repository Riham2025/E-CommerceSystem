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
        public string? Phone { get; set; }

        [Required]
        public int SupplierId { get; set; }

        // === Nested DTOs (as classes) for AutoMapper ===

        // Create
        public class SupplierCreateDto
        {
            [Required]
            public string Name { get; set; } = default!;

            [EmailAddress]
            public string? ContactEmail { get; set; }

            [Phone]
            public string? Phone { get; set; }
        }

        // Update
        public class SupplierUpdateDto
        {
            [Required]
            public string Name { get; set; } = default!;

            [EmailAddress]
            public string? ContactEmail { get; set; }

            [Phone]
            public string? Phone { get; set; }
        }

        // Read
        public class SupplierReadDto
        {
            [Required]
            public int SupplierId { get; set; }

            [Required]
            public string Name { get; set; } = default!;

            [EmailAddress]
            public string? ContactEmail { get; set; }

            [Phone]
            public string? Phone { get; set; }
        }
    }
}
