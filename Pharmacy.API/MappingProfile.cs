using AutoMapper;
using Pharmacy.API.Dtos;
using Pharmacy.Domian.Entities.OrderAggregate;

namespace Pharmacy.API
{
    public class MappingProfile : Profile
    {
        protected MappingProfile()
        {
            CreateMap<Address, AddressDto>().ReverseMap() ;
            CreateMap<AddressDto, Pharmacy.Domian.Entities.OrderAggregate.Address>();
         
        }
    }
}
