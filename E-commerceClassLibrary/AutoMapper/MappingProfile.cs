using AutoMapper;
using E_commerceClassLibrary.DTO.Sales;
using E_commerceClassLibrary.Models.Sales;

namespace E_commerceClassLibrary.AutoMapper
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<CreateUpdateCustomer, Customer>();
            CreateMap<Customer, CustomerDTO>();
        }
    }
}
