using AutoMapper; 
using E_CommerceSystem.Models; 
using E_CommerceSystem.Models;   

namespace E_CommerceSystem.Mapping
{
    public class CategoryProfile : Profile 
    {
        public CategoryProfile() 
        {
            // Create → Entity
            CreateMap<CategoryDTO.CategoryCreateDto, Category>();

            // Update → Entity (Map(dto, entity))
            CreateMap<CategoryDTO.CategoryUpdateDto, Category>();

            // Entity → Read
            CreateMap<Category, CategoryDTO.CategoryReadDto>();
        }
    }
}
