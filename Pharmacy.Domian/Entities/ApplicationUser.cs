using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pharmacy.Domian.Entities
{
    public class ApplicationUser : IdentityUser
    {
        public List<Product> Products { get; set; }
        public List<WishList> WishLists { get; set; }
    }
}
