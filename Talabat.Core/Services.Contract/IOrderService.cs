using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities.Order_Aggregate_Modul;

namespace Talabat.Core.Services.Contract
{
    // 4 Methods signitures
    public interface IOrderService
    {
        Task<Order?>CreateOrderAsync(string buyerEmail,string basketId,int deliveryMethodId, Address ShippingAddress );


        Task<IReadOnlyList<Order>> GetOrdersForUserAsync(string buyerEmail);


        Task<Order?> GetOrderbyIdForUserAsync(int orderId,string buyerEmail);

        Task<IReadOnlyList<DeliveryMethod>> GetDeliveryMethodsAsync();


    }
}
