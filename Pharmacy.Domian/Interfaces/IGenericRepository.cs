using Pharmacy.Domian.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pharmacy.Domian.Interfaces
{
    public interface IGenericRepository<T> where T : BaseEntity
    {
        Task<T> GetByIdAsync(int id);
        Task<IReadOnlyList<T>> LisAllAsync();
        //Task<T> GetEntityWithSpec(ISpecification<T> spec);
        //Task<IReadOnlyList<T>> ListAsync(ISpecification<T> spec);
        //Task<int> CountAsync(ISpecification<T> spec);
        void Add(T entity);
        void Update(T entity);
        void Delete(T entity);
    }
}
