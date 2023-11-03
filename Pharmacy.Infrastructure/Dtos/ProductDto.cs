using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pharmacy.Infrastructure.Dtos
{
    public class ProductDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal price { get; set; }
        public string ImageUrl { get; set; }
        public string ProductType { get; set; }
        
        

    }
}
