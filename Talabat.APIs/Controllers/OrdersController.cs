using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Talabat.APIs.Dtos;
using Talabat.APIs.Errors;
using Talabat.Core.Entities.Order_Aggregate_Modul;
using Talabat.Core.Services.Contract;

namespace Talabat.APIs.Controllers
{
    [Authorize] //All endpoints must be Authorize 
    public class OrdersController : BaseApiController
    {
        private readonly IOrderService _orderService;
        private readonly IMapper _mapper;

        ///To Connect on Methods of order service must take object from OrderService
        ///To take object from OrderService Must allow DI to IOrderService

        public OrdersController(
            IOrderService orderService
            , IMapper mapper         //Mapping(copy) data from addressDto to Address 

            )
        {
            _orderService = orderService;
            _mapper = mapper;
        }

        ///what is difference between OrderDto and OrderToreturnDto
        ///OrderToreturnDto : Order will make it return at reponse 
        ///OrderDto :Order parameters to CreateOrder 

        //Improvment to Swagger Documentation
        [ProducesResponseType(typeof(OrderToReturnDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
        [HttpPost]  //Post: /api/orders
        public async Task<ActionResult<OrderToReturnDto>> CreateOrder(OrderDto orderDto)
        {
            //User: This property inside it Claims of Token and inherited from ControllerBase

            var buyerEmail = User.FindFirstValue(ClaimTypes.Email);

            var address = _mapper.Map<AddressDto, Address>(orderDto.ShippingAddress);
            var order = await _orderService.CreateOrderAsync(buyerEmail, orderDto.BasketId, orderDto.DeliveryMethodId, address);

            if (order is null)
                return BadRequest(new ApiResponse(400));

            return Ok(_mapper.Map<Order,OrderToReturnDto>(order));
        }

    
        [HttpGet]  // Get: /api/Orders
        public async Task<ActionResult<IReadOnlyList<OrderToReturnDto>>>GetOrdersForUser()
             {
            var buyerEmail = User.FindFirstValue(ClaimTypes.Email);

            var orders =await _orderService.GetOrdersForUserAsync(buyerEmail);

            return Ok(_mapper.Map<IReadOnlyList<Order>, IReadOnlyList<OrderToReturnDto>>(orders));

            }

        [ProducesResponseType(typeof(OrderToReturnDto),StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse),StatusCodes.Status404NotFound)]
        [HttpGet("{id}")]  // Get: /api/Orders/1
        public async Task<ActionResult<IReadOnlyList<OrderToReturnDto>>> GetOrderForUser(int id) //id => id of order
        {
            var buyerEmail = User.FindFirstValue(ClaimTypes.Email);

             
            var order = await _orderService.GetOrderbyIdForUserAsync(id, buyerEmail);

            if (order is null)  return NotFound(new ApiResponse(404));

            //Map => Has Two copies
            return Ok(_mapper.Map<OrderToReturnDto>(order));
       
        }

        //Get All Delivery Methods that exist at System
        //"deliveryMethods" => static Segment 
        [HttpGet("deliveryMethods")] // Get: /api/Orders/deliveryMethods

        public async Task<ActionResult<IReadOnlyList<DeliveryMethod>>> GetDeliveryMethods()
        {
            var deliveryMethod = await _orderService.GetDeliveryMethodsAsync();

            return Ok(deliveryMethod);
        }

    }
}
