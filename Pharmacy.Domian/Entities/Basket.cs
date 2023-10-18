using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pharmacy.Domian.Entities
{
    public class Basket : BaseEntity
    {
        public string Addresses { get; set; }
        public List<BasketItem> Items { get; set; }
    }
}
