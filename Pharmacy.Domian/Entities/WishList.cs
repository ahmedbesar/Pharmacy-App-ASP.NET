using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Pharmacy.Domian.Entities.Identity;

namespace Pharmacy.Domian.Entities
{
    public class WishList:BaseEntity
    {
     
        public Product Product { get; set; }
        public int ProductId { get; set; }
        public ApplicationUser User { get; set; }
        public string UserId { get; set; }

    }
}
