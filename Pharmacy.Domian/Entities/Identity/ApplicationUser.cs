using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pharmacy.Domian.Entities.Identity
{
    public class ApplicationUser : IdentityUser
    {
        public string ProfileImage { get; set; }
        public string Location { get; set; }
        public List<RefreshToken>? RefreshTokens { get; set; }
        public List<Product> Products { get; set; }
        public List<WishList> WishLists { get; set; }
    }
}
