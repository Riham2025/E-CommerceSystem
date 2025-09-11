using AutoMapper;
using E_CommerceSystem.Models;   // SupplierDTO nested types + Entity: Supplier

namespace E_CommerceSystem.Mapping
{
    public class SupplierProfile : Profile
    {
        public SupplierProfile()
        {
            CreateMap<SupplierDTO.SupplierCreateDto, Supplier>(); // Create → Entity
            CreateMap<SupplierDTO.SupplierUpdateDto, Supplier>(); // Update → Entity (Map(dto, entity))
            CreateMap<Supplier, SupplierDTO.SupplierReadDto>(); // Entity → Read
        }
    }
}
