using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pharmacy.Domian.Entities.Identity
{
    public class ApplicationUser : IdentityUser
    {
        [MaxLength(4),MinLength(4)]
        public string VerificationCode { get; set; }
        public string ProfileImage { get; set; }
        public string Location { get; set; }
        public List<RefreshToken>? RefreshTokens { get; set; }
        public List<Product> Products { get; set; }
        public List<WishList> WishLists { get; set; }
    }
}
