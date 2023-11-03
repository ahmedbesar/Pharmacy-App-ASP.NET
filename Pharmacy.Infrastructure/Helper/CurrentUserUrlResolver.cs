using AutoMapper;
using Microsoft.Extensions.Configuration;
using Pharmacy.Domian.Entities;
using Pharmacy.Domian.Entities.Identity;
using Pharmacy.Domian.IdentityDtos;
using Pharmacy.Infrastructure.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pharmacy.Infrastructure.Helper
{
    public class CurrentUserUrlResolver : IValueResolver<ApplicationUser, UserDto, string>
    {
        private readonly IConfiguration _config;
        public CurrentUserUrlResolver(IConfiguration config)
        {
            _config = config;
        }

        public string Resolve(ApplicationUser source, UserDto destination, string destMember, ResolutionContext context)
        {
            if (!string.IsNullOrEmpty(source.ProfileImage))
            {
                return _config["ApiUrl"] + "uploads/images/users/" + source.ProfileImage;
            }

            return null;

        }
    }
}
