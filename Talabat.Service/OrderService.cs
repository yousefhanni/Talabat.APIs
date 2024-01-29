using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core;
using Talabat.Core.Entities;
using Talabat.Core.Entities.Order_Aggregate_Modul;
using Talabat.Core.Repositories.Contract;
using Talabat.Core.Services.Contract;
using Talabat.Core.Specifications.Order_Specs;
using Talabat.Core.Specifications.OrderSpecs;

namespace Talabat.Service
{
    //OrderService => represent (Repo of Orders) that contain all Methods of Order  
    public class OrderService : IOrderService
    {
        private readonly IBasketRepository _basketRepo;
        private readonly IUnitOfWork _unitOfWork; // ALL interactions with DB are through (_unitOfWork)
        private readonly IPaymentService _paymentService;

        ///private readonly IGenericRepository<Product> _productsRepo;
        ///private readonly IGenericRepository<DeliveryMethod> _deliveryMethodsRepo;
        ///private readonly IGenericRepository<Order> _ordersRepo;

        ///OrderService no deal with repositories Direct but deal with UnitofWork
        ///that will get repository per Reque

        //Take object from class that implement interface (IBasketRepository) 
        public OrderService(
            IBasketRepository basketRepo,
            IUnitOfWork unitOfWork,
            IPaymentService paymentService

            ///IGenericRepository<Product> ProductsRepo,
            ///IGenericRepository<DeliveryMethod> deliveryMethodsRepo,
            ///IGenericRepository<Order> OrdersRepo
            
            )
        {
           _basketRepo = basketRepo;
            _unitOfWork = unitOfWork;
           _paymentService = paymentService;
            /// _productsRepo = ProductsRepo;
            ///_deliveryMethodsRepo = deliveryMethodsRepo;
            /// _ordersRepo = OrdersRepo;
        }
        public async Task<Order?> CreateOrderAsync(string buyerEmail, string basketId, int deliveryMethodId, Address ShippingAddress)
        {
            // 1. Get Basket From Baskets Repo
            var basket = await _basketRepo.GetBasketAsync(basketId);

            // 2. Get Selected Items at Basket From Products Repo

            //Empty List of OrderItems 
            var orderItems = new List<OrderItems>();

            //If there are items in the basket
            if (basket?.Items?.Count > 0)
            {
                ///I Use(take) From basketItems (Id and Quantity) only
                ///and rest info about item that exist at basket,i will Fetch(get) rest info From ProductRepo
                ///Which From  Product that exist at Database=> Because Front may is put Incorrect data about product
                ///and To Put(Order) Correct data  about Product must fetch this data from Database 
              
                var productRepository = _unitOfWork.Repository<Product>();
                foreach (var item in basket.Items)
                {      
                    //item.Id:each item in the basket has (Id) that corresponds to a product in your product repository.
                    var product = await productRepository.GetByIdAsync(item.Id);
                    var productItemOrdered = new ProductItemOrdered(item.Id,product.Name, product.PictureUrl);

                    //Quantity : That exist at Basket 
                    var orderItem = new OrderItems(productItemOrdered, product.Price, item.Quantity);

                   //Add method exist at List that Add Created orderItems to the orderItems List
                    orderItems.Add(orderItem);
                }
            }

            // 3. Calculate SubTotal

            //Sum method is LINQ operates by iterating over the elements in the collection. 
            var subtotal = orderItems.Sum(orderItem => orderItem.Price * orderItem.Quantity);

            // 4. Get Delivery Method From DeliveryMethods Repo

            var deliveryMethod = await _unitOfWork.Repository<DeliveryMethod>().GetByIdAsync(deliveryMethodId);

            var ordersRepo = _unitOfWork.Repository<Order>();

            var orderSpecs = new OrderWithPaymentIntentSpecifications(basket.PaymentIntentId);

            var existingOrder = await ordersRepo.GetEntityWithSpecAsync(orderSpecs);

            //Check if there old Order with This PaymentIntentId to Delete This Order from Basket

            if (existingOrder != null)
            {
                ordersRepo.DeleteAsync(existingOrder);

                await _paymentService.CreateOrUpdatePaymentIntent(basketId);
            }

            // 5. Create Order

            var order = new Order(buyerEmail, ShippingAddress, deliveryMethod, orderItems, subtotal,basket.PaymentIntentId);

            //Add Order at OrderRepo(DB)

            await ordersRepo.AddAsync(order);

            // 6. Save To Database [TODO]

           var result = await  _unitOfWork.CompleteAsync(); //Here Save changes one time for all operations
            if (result <= 0) return null; //There are no orders created

            return order;  //There are orders created
        }

        public async Task<IReadOnlyList<Order>> GetOrdersForUserAsync(string buyerEmail)
        {
            var  ordersRepo = _unitOfWork.Repository<Order>();   
            // Use Criteria(Where) That inside Specification to Filteration with (buyerEmail)  
            var spec = new OrderSpecifications(buyerEmail);
            
            var orders = await ordersRepo.GetAllWithSpecAsync(spec);

            return orders;
        }

        public Task<Order?> GetOrderbyIdForUserAsync(int orderId, string buyerEmail)
        {
            var ordersRepo = _unitOfWork.Repository<Order>();

            var orderspec = new OrderSpecifications(orderId, buyerEmail);

            var order = ordersRepo.GetEntityWithSpecAsync(orderspec);
              
            return order;
        }

        public Task<IReadOnlyList<DeliveryMethod>> GetDeliveryMethodsAsync()
        {
           var deliveryMethodsRepo = _unitOfWork.Repository<DeliveryMethod>();
           //Use GetAllAsync not GetAllWithspec because Not Exist (N.P) at DeliveryMethod
            var deliveryMethods = deliveryMethodsRepo.GetAllAsync(); //        
            return deliveryMethods;

        }
    }
}
