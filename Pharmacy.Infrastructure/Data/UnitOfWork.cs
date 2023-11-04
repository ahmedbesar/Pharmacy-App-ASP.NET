using Pharmacy.Domian.Entities;
using Pharmacy.Domian.Interfaces;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pharmacy.Infrastructure.Data
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly StoreContext _context;
        private Hashtable _repositories;
        public UnitOfWork(StoreContext context)
        {
            _context = context;
        }

        public Task<int> Complete()
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            _context.Dispose();
        }

        public IGenericRepository<TEntity> Repository<TEntity>() where TEntity : BaseEntity
        {
            if (_repositories == null) _repositories = new Hashtable();
            var type = typeof(TEntity).Name;

            if (!_repositories.ContainsKey(type))
            {
                var respositoryType = typeof(GenericRepository<>);
                var respositoryInstance = Activator
                        .CreateInstance(respositoryType.MakeGenericType(typeof(TEntity)), _context);

                _repositories.Add(type, respositoryInstance);
            }

            return (IGenericRepository<TEntity>)_repositories[type];
        }
    }
}
