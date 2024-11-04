using AutoMapper;
using E_commerceClassLibrary.DTO.Sales;
using E_commerceClassLibrary.Models.Sales;

namespace E_commerceClassLibrary.AutoMapper
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<CreateUpdateCartItemDTO, CartItem>();
            CreateMap<ReadCartItemDTO, CartItem>();
            CreateMap<CartItem, ReadCartItemDTO>()
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Product.Name))
                .ForMember(dest => dest.Price, opt => opt.MapFrom(src => src.Product.Price));

            CreateMap<CreateUpdateCustomerDTO, Customer>();
            CreateMap<Customer, ReadCustomerDTO>();

            CreateMap<CreateUpdateStaffDTO, Staff>();
            CreateMap<Staff, ReadStaffDTO>();

            CreateMap<CreateUpdateOrderDTO, Order>();
            CreateMap<Order, ReadOrderDTO>();
        }
    }
}
