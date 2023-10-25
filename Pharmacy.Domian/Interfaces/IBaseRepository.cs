using Pharmacy.Domian.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Pharmacy.Domian.Interfaces
{
    public interface IBaseRepository<T> where T : BaseEntity
    {
        Task<IEnumerable<T>> FindAllAsync(Expression<Func<T, bool>> criteria, int pgnum, int pgsize);
        T GetById(int id);
        Task<T> GetByIdAsync(Expression<Func<T, bool>> criteria,  string[]? includes = null);
        Task<IEnumerable<T>> GetAllAsyncWithPagination(int pgnum, int pgsize, string[] includes = null);
        T Add(T entity);
        Task<T> AddAsync(T entity);
        IEnumerable<T> AddRange(IEnumerable<T> entities);
        Task<IEnumerable<T>> AddRangeAsync(IEnumerable<T> entities);
        T Update(T entity);
        void Delete(T entity);
        void DeleteRange(IEnumerable<T> entities);
        void Attach(T entity);
        void AttachRange(IEnumerable<T> entities);
        int Count();
        int Count(Expression<Func<T, bool>> criteria);
        Task<int> CountAsync();
        Task<int> CountAsync(Expression<Func<T, bool>> criteria);


        /// wishlist
       
       // WishList Create_(WishList WishList);
        List<WishList>GetUserWishList(string userId, int pgnum, int pgsize);
     


        //product
            
        List<Product> GetAllProductWithSameType(int id, int pgnum, int pgsize);
        List<Product> GetByProductNameOrProductTypeNameWithPagination(string term, int pgnum, int pgsize);

        List<Product> GetProductsWithSameProductTypeAndSearchUsingNameAndPagination(int ProductTypeId, string term, int pgnum, int pgsize);

        List<Product> GetRecommendedProductsWithPagination(int id, int pgnum, int pgsize);
        //

        //type
        bool IsvalidProductType(int id);
        List<ProductType> GetByProductTypeNameAndSearchWithPagination(string? term, int pgnum, int pgsize);
        //
    }
}
