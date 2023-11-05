using Microsoft.EntityFrameworkCore;
using Pharmacy.Domian.Entities;
using Pharmacy.Domian.Entities.OrderAggregate;
using Pharmacy.Domian.Interfaces;
using Pharmacy.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pharmacy.Services
{
    public class OrderService : IOrderService
    {
        private readonly IGenericRepository<Order> _orderRepo;
        private readonly IGenericRepository<DeliveryMethod> _dmRepo;
        private readonly IBasketRepository _basketRepo;
        private readonly IGenericRepository<Product> _productRepo;
        private readonly ApplicationDbContext _context;

        public OrderService(ApplicationDbContext context)
        {
           _context = context;
        }

        public OrderService(IGenericRepository<Order> orderRepo, IGenericRepository<DeliveryMethod> dmRepo,
            IBasketRepository basketRepo, IGenericRepository<Product> productRepo)
        {
           _orderRepo = orderRepo;
            _dmRepo = dmRepo;
            _basketRepo = basketRepo;
            _productRepo = productRepo;
        }

        public async Task<Order> CreateOrderAsync(string buyerEmail, int deliveryMethodId, string basketId, Address shippingAddress)
        {
            //get the basket
            var basket = await _basketRepo.GetBasketAsync(basketId);

            // get the items from product repo
            var items = new List<OrderItem>();
            foreach (var item in basket.Items)
            {
                var productItem = await _productRepo.GetByIdAsync(item.Id);
                var itemOrdered = new ProductItemOrdered(productItem.Id, productItem.Name,
                        productItem.ImageUrl);
                var orderItem = new OrderItem(itemOrdered, productItem.price, item.Quntity);
                items.Add(orderItem);
            }

            // get delivery method from repo
            var deliveryMethod = await _dmRepo.GetByIdAsync(deliveryMethodId);

            //calc subtotal
            var subtotal = items.Sum(item => item.Price * item.Quantity);

            //create order
            var order = new Order(items, buyerEmail, shippingAddress, deliveryMethod, subtotal);


            return order;
        }

        

        public Task<IReadOnlyList<DeliveryMethod>> GetDeliveryMethodAsync()
        {
            throw new NotImplementedException();
        }

        public async Task<Order> GetOrderById(int id, string buyerEmail)
        {
            return await _context.orders
            .Where(order => order.Id == id && order.BuyerEmail == buyerEmail)
            .SingleOrDefaultAsync();
        }

        public Task<IReadOnlyList<Order>> GetOrdersForUserAsync(string buyerEmail)
        {
            throw new NotImplementedException();
        }
    }
}
