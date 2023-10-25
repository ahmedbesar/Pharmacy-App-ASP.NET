using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pharmacy.Domian.Entities
{
    public class Product : BaseEntity
    {
       
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal  price { get; set; }
        public string ImageUrl { get; set; }
        public ProductType ProductType { get; set; }
        public int ProductTypeId { get; set; }
        public List<WishList> WishLists { get; set; }


    }
}
