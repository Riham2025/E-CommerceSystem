using AutoMapper;
using E_CommerceSystem.Models;   // SupplierDTO nested types + Entity: Supplier

namespace E_CommerceSystem.Mapping
{
    public class SupplierProfile : Profile
    {
        public SupplierProfile()
        {
            CreateMap<SupplierDTO.SupplierCreateDto, Supplier>();
            CreateMap<SupplierDTO.SupplierUpdateDto, Supplier>();
            CreateMap<Supplier, SupplierDTO.SupplierReadDto>();
        }
    }
}
