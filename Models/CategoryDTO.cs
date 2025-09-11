using System.ComponentModel.DataAnnotations;

namespace E_CommerceSystem.Models
{
    public class CategoryDTO // Data Transfer Object (DTO) for Category
    {
        [Required]
        public string Name { get; set; } = default!; // Non-nullable

        public string? Description { get; set; } // Nullable

        [Required] // Primary Key
        public int CategoryId { get; set; } // Primary Key

        // === Nested DTOs (as classes) for AutoMapper ===

        // Create
        public class CategoryCreateDto 
        {
            [Required]
            public string Name { get; set; } = default!; // Non-nullable

            public string? Description { get; set; } // Nullable
        }

        // Update
        public class CategoryUpdateDto
        {
            [Required] // Primary Key
            public string Name { get; set; } = default!; // Non-nullable

            public string? Description { get; set; } // Nullable
        }

        // Read
        public class CategoryReadDto
        {
            [Required] // Primary Key
            public int CategoryId { get; set; } // Primary Key

            [Required]
            public string Name { get; set; } = default!; // Non-nullable

            public string? Description { get; set; } // Nullable
        }
    }
}
