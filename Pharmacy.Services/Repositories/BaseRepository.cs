using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using Pharmacy.Domian.Entities;
using Pharmacy.Domian.Entities.Identity;
using Pharmacy.Domian.Interfaces;
using Pharmacy.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Pharmacy.Services.Repositories
{
    public class BaseRepository<T> : IBaseRepository<T> where T : BaseEntity
    {
        protected ApplicationDbContext _context;

        public BaseRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        
        public async Task<IEnumerable<T>> FindAllAsync(Expression<Func<T, bool>> criteria , int pgnum, int pgsize)
        {

            var query = await _context.Set<T>().Where(criteria).ToListAsync();
            return query.Skip(pgnum * pgsize - pgsize).Take(pgsize);
        }
       
        public async Task<IEnumerable<T>> GetAllAsyncWithPagination(int pgnum,int pgsize, string[] includes = null)
        {
           
            IQueryable<T> query = _context.Set<T>();
            if (includes != null)
                foreach (var include in includes)
                    query = query.Include(include);

            return await query.Skip(pgnum * pgsize - pgsize).Take(pgsize).ToListAsync();
        }
        public T GetById(int id)
        {
            return _context.Set<T>().Find(id);
        }

        public async Task<T> GetByIdAsync(Expression<Func<T, bool>> criteria, string[]? includes = null)
        {
            if (includes == null)
                return await _context.Set<T>().SingleOrDefaultAsync(criteria);



            IQueryable<T> query = _context.Set<T>();
            if (includes != null)
                foreach (var include in includes)
                    query = query.Include(include);

            return await query.SingleOrDefaultAsync(criteria);
            
        }


        //only for product 
      

        public List<Product> GetAllProductWithSameType(int id, int pgnum, int pgsize)
        {
              var Products = _context.Product.Include(a => a.ProductType)
                .Where(a => a.ProductTypeId== id).Skip(pgnum * pgsize - pgsize).Take(pgsize).ToList();
                return Products;
           
        }
        public bool IsvalidProductType(int id)
        {
            return _context.ProductType.Any(g => g.Id == id);
        }
        public bool IsvalidProduct(int id)
        {
            return _context.Product.Any(g => g.Id == id);
        }
        public List<Product> GetByProductNameOrProductTypeNameWithPagination(string? term, int pgnum, int pgsize)
        {
            if (term != null)
            {
               var  result = _context.Product.Include(a => a.ProductType)
                .Where(a => a.Name.Contains(term)
                ||a.ProductType.Name.Contains(term)).Skip(pgnum * pgsize - pgsize).Take(pgsize).ToList();
                return result;
            }
            else
            { 
                var result = _context.Product.Include(a => a.ProductType)
               .Skip(pgnum * pgsize - pgsize).Take(pgsize).ToList();
                return result;
            }
       
        }
        public List<Product> GetProductsWithSameProductTypeAndSearchUsingNameAndPagination(int ProductTypeId, string term, int pgnum, int pgsize)
        {
            var result = _context.Product.Include(a => a.ProductType)
                .Where(a => a.Name.Contains(term)
                && a.ProductType.Id== ProductTypeId).Skip(pgnum * pgsize - pgsize).Take(pgsize).ToList();
            return result;

        }

        public Product GetProductById(int id)
        {
            return _context.Product.Find(id);
        }

        public List<Product> GetRecommendedProductsWithPagination(int id,int pgnum, int pgsize)
        {
                var _Product = GetProductById(id);
         
                
                var Recommended = _context.Product.Include(a => a.ProductType).Where(a => a.ProductTypeId==_Product.ProductTypeId && a.price > _Product.price - 100
                && a.price < _Product.price + 100).Skip(pgnum * pgsize - pgsize).Take(pgsize).OrderByDescending(a=>a.price).ToList();
                return Recommended;
            
        }
        
        public T Add(T entity)
        {
            _context.Set<T>().Add(entity);
            return entity;
        }

        public async Task<T> AddAsync(T entity)
        {
            await _context.Set<T>().AddAsync(entity);
            return entity;
        }

        public IEnumerable<T> AddRange(IEnumerable<T> entities)
        {
            _context.Set<T>().AddRange(entities);
            return entities;
        }

        public async Task<IEnumerable<T>> AddRangeAsync(IEnumerable<T> entities)
        {
            await _context.Set<T>().AddRangeAsync(entities);
            return entities;
        }

        public T Update(T entity)
        {
            _context.Update(entity);
            return entity;
        }

        public void Delete(T entity)
        {
            _context.Set<T>().Remove(entity);
        }

        public void DeleteRange(IEnumerable<T> entities)
        {
            _context.Set<T>().RemoveRange(entities);
        }

        public void Attach(T entity)
        {
            _context.Set<T>().Attach(entity);
        }

        public void AttachRange(IEnumerable<T> entities)
        {
            _context.Set<T>().AttachRange(entities);
        }

        public int Count()
        {
            return _context.Set<T>().Count();
        }

        public int Count(Expression<Func<T, bool>> criteria)
        {
            return _context.Set<T>().Count(criteria);
        }

        public async Task<int> CountAsync()
        {
            return await _context.Set<T>().CountAsync();
        }

        public async Task<int> CountAsync(Expression<Func<T, bool>> criteria)
        {
            return await _context.Set<T>().CountAsync(criteria);
        }

        public WishList Create_(WishList WishList)
        {
           var newWishlist=  _context.WishLists.Add(WishList);

            return WishList;
        }
     
        public List<WishList> GetUserWishList(string userId, int pgnum, int pgsize)
        {
            var query = _context.WishLists
                .Include(w => w.Product).ThenInclude(w => w.ProductType)
                .Where(w => w.UserId.Equals(userId));
            return query.Skip((pgnum * pgsize) - pgsize).Take(pgsize).ToList();
                
        }

        public WishList GetWishListById(int id)
        {
            return _context.WishLists.FirstOrDefault(w => w.Id == id);
        }
        public async Task<WishList> checkProductInUserWishlist(string userId, int productId)
        {
            var wishList = await _context.WishLists.FirstOrDefaultAsync(u => u.UserId==userId&& u.ProductId==productId);
            
            return wishList;
        }
        public void Remove_ (WishList WishList)
        {
             _context.WishLists.Remove(WishList);
        }

        List<ProductType> IBaseRepository<T>.GetByProductTypeNameAndSearchWithPagination(string? term, int pgnum, int pgsize)
        {
            if (term != null)
            {
                var result = _context.ProductType
                 .Where(a => a.Name.Contains(term)).Skip((pgnum * pgsize) - pgsize).Take(pgsize).ToList();
                return result;
            }
            else
            {
                var result = _context.ProductType
               .Skip((pgnum * pgsize - pgsize)).Take(pgsize).ToList();
                return result;
            }
        }
    }
}
