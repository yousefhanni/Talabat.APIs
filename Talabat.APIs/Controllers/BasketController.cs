using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Talabat.APIs.Dtos;
using Talabat.APIs.Errors;
using Talabat.Core.Entities;
using Talabat.Core.Repositories.Contract;

namespace Talabat.APIs.Controllers
{

    public class BasketController : BaseApiController
    {
        private readonly IBasketRepository _basketRepository;
        private readonly IMapper _mapper;

        public BasketController(IBasketRepository basketRepository,
            IMapper mapper
            
            )
        {
            _basketRepository = basketRepository;
            _mapper = mapper;
        }

        [HttpGet] //Get: /api/baseket?id=

        public async Task<ActionResult<CustomerBasket>>GetBasket(string Id)
         {
            var basket = await _basketRepository.GetBasketAsync(Id);

            return Ok(basket ?? new CustomerBasket(Id));   //if CustomerBasket null,so item expired, so create new basket with the same id then return this
        }

        [HttpPost]  //Post: /api/baseket
     
        public async Task<ActionResult<CustomerBasket>> UpdateBasket(CustomerBasketDto basket)
        {   
         ///I used CustomerBasketDto to just only validations  
            ///Mapping:process of copying data from one object to another
            ///Copy Data from the CustomerBasketDto to the CustomerBasket,
            ///you ensure that the validation logic within the CustomerBasket is applied before the data is used 

            var mappedBasket = _mapper.Map<CustomerBasketDto, CustomerBasket>(basket);

            //mappedBasket:object of CustomerBasket contains the data that has been mapped from the original CustomerBasketDto.
            var CreatedOrUpdatedBasket = await _basketRepository.UpdateBasketAsync(mappedBasket);

            if (CreatedOrUpdatedBasket is null)  return BadRequest(new ApiResponse(400));

            return Ok(CreatedOrUpdatedBasket);
          }

        [HttpDelete]  //Delete: /api/basket?id=

        public async Task DeleteBasket(string Id) 
        {

            await _basketRepository.DeleteBasketAsync(Id);

        }


    }
}
