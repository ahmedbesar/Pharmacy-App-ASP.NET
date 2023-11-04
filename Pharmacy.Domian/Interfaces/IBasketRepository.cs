using Pharmacy.Domian.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pharmacy.Domian.Interfaces
{
    public interface IBasketRepository
    {
        //Task<IReadOnlyList<Basket>> GetAll();
        Task<Basket> GetBasketAsync(string basketId);
        Task<Basket> UpdateBasketAsync(Basket basket);
        Task<bool> DeleteBasketAsync(string basketId);
    }
}
