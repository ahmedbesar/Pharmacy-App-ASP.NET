using Pharmacy.Domian.Entities;
using Pharmacy.Domian.Interfaces;
using Pharmacy.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pharmacy.Services.Repositories
{
    public class ProductRepository : BaseRepository<Product>, IProductRepository
    {
        private readonly ApplicationDbContext _context;

        public ProductRepository(ApplicationDbContext context) : base(context)
        {
           
        }
       

        public IEnumerable<Product> GetAlllllll111()
        {
           return _context.Product.ToList();
        }

        IEnumerable<Product> IProductRepository.GetAlllllll111()
        {
            throw new NotImplementedException();
        }
    }
}
