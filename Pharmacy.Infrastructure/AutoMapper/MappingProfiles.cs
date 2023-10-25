using AutoMapper;
using Pharmacy.Infrastructure.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Pharmacy.Domian.Entities;
using Pharmacy.Infrastructure.Helper;

namespace Pharmacy.Infrastructure.AutoMapper
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            CreateMap<Product, ProductDto>()
                .ForMember(x => x.ProductType, y => y.MapFrom(z => z.ProductType.Name))
                .ForMember(x => x.ImageUrl, y => y.MapFrom<ProductUrlResolver>());

            CreateMap<ProductType, ProductTypeDto>()
                .ForMember(x => x.ImageUrl, y => y.MapFrom<ProductTypeUrlResolver>());

            CreateMap<WishList, WishListDto>()
                  ;
        }
    }
}
