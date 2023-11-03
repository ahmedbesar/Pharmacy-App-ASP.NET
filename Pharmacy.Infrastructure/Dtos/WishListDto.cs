using Pharmacy.Domian.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pharmacy.Infrastructure.Dtos
{
    public class WishListDto
    {
        public int Id { get; set; }
        public ProductDto Product { get; set; }
        
    }
}
