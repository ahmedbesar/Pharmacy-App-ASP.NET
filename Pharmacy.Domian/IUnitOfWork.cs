using Pharmacy.Domian.Entities;
using Pharmacy.Domian.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pharmacy.Domian
{
    public interface IUnitOfWork : IDisposable
    {
        IBaseRepository<Product> Products { get; }
        IBaseRepository<ProductType> ProductTypes { get; }
        IBaseRepository<WishList> WishLists { get; }
        
        int Complete();
    }
}