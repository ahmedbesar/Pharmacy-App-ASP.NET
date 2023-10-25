using AutoMapper;
using Microsoft.Extensions.Configuration;
using Pharmacy.Domian.Entities;
using Pharmacy.Infrastructure.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pharmacy.Infrastructure.Helper
{
    public class ProductTypeUrlResolver : IValueResolver<ProductType, ProductTypeDto, string>
    {
        private readonly IConfiguration _config;
        public ProductTypeUrlResolver(IConfiguration config)
        {
            _config = config;
        }

        public string Resolve(ProductType source, ProductTypeDto destination, string destMember, ResolutionContext context)
        {
            if (!string.IsNullOrEmpty(source.ImageUrl))
            {
                return _config["ApiUrl"] + source.ImageUrl;
            }

            return null;

        }

    }
}
