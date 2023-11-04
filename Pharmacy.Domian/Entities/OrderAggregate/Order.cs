using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pharmacy.Domian.Entities.OrderAggregate
{
    public class Order : BaseEntity
    {
        public Order()
        {
        }

        public Order(IReadOnlyList<OrderItem> item, string buyerEmail,
            Address shipToAddress, DeliveryMethod deliveryMethod,
            decimal subtotal)
        {
            BuyerEmail = buyerEmail;
            ShipToAddress = shipToAddress;
            DeliveryMethod = deliveryMethod;
            OrderItems = item;
            Subtotal = subtotal;
          
        }

        public string BuyerEmail { get; set; }
        public DateTimeOffset dateTimeOffset { get; set; }
        [NotMapped]
        public Address? ShipToAddress { get; set; }
        public DeliveryMethod DeliveryMethod { get; set; }
        public OrderStatus Status { get; set; } = OrderStatus.Pinding;
        public IReadOnlyList <OrderItem> OrderItems{ get; set; } 
        public decimal Subtotal { get; set; }
        public string PaymentIntenId { get; set; }

        public decimal GetTotal()
        {
            return Subtotal + DeliveryMethod.Price;
        }

    }
}
