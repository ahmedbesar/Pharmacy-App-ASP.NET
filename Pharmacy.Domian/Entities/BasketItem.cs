using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pharmacy.Domian.Entities
{
    public class BasketItem : BaseEntity
    {
        public int ProductId { get; set; }
        public string Name { get; set; }

        public string Description { get; set; }
        public decimal price { get; set; }
        public string ImageUrl { get; set; }
        public string Type { get; set; } 
        public int Quntity { get; set; }
    }
}
