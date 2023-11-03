using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Pharmacy.Domian.Entities.OrderAggregate
{
    public enum OrderStatus
    {
        [EnumMember(Value= "Pinding")]
        Pinding,

        [EnumMember(Value = "Payment Received")]
        PaymentReceived,

        [EnumMember(Value = "Payment Failed")]
        paymentFailed
    }
}
