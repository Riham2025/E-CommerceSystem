using System.ComponentModel.DataAnnotations;

namespace E_CommerceSystem.Models
{
    public class CategoryDTO // Data Transfer Object (DTO) for Category
    {
        [Required]
        public string Name { get; set; } = default!; // Non-nullable

        public string? Description { get; set; } // Nullable

        [Required] // Primary Key
        public int CategoryId { get; set; }

        // === Nested DTOs (as classes) for AutoMapper ===

        // Create
        public class CategoryCreateDto
        {
            [Required]
            public string Name { get; set; } = default!;

            public string? Description { get; set; }
        }

        // Update
        public class CategoryUpdateDto
        {
            [Required]
            public string Name { get; set; } = default!;

            public string? Description { get; set; }
        }

        // Read
        public class CategoryReadDto
        {
            [Required]
            public int CategoryId { get; set; }

            [Required]
            public string Name { get; set; } = default!;

            public string? Description { get; set; }
        }
    }
}
