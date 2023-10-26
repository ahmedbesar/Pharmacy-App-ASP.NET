using Pharmacy.Domian;
using Pharmacy.Domian.Entities;
using Pharmacy.Domian.Interfaces;
using Pharmacy.Services.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Reflection.Metadata.BlobBuilder;

namespace Pharmacy.Services
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _context;

        public IBaseRepository<Product> Products { get; private set; }
        public IBaseRepository<ProductType> ProductTypes { get; private set; }
        public IBaseRepository<WishList> WishLists { get; private set; }


        public UnitOfWork(ApplicationDbContext context)
        {
            _context = context;

            Products = new BaseRepository<Product>(_context);
            ProductTypes = new BaseRepository<ProductType>(_context);
            WishLists = new BaseRepository<WishList>(_context);
            
        }

        public int Complete()
        {
            return _context.SaveChanges();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}