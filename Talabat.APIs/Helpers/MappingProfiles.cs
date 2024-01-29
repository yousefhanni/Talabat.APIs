using AutoMapper;
using Talabat.APIs.Dtos;
using Talabat.Core.Entities;
using Talabat.Core.Entities.Identity;
using Talabat.Core.Entities.Order_Aggregate_Modul;

namespace Talabat.APIs.Helpers
{
    //when create object from this class => execute constructor
    public class MappingProfiles:Profile
    {       
        public MappingProfiles()
        {


            CreateMap<Product, ProductToReturnDto>()
                .ForMember(d => d.Brand, O => O.MapFrom(s => s.Brand.Name)) //there you say to it that brand that exist at Destination as string ,Map value From Name
                .ForMember(d => d.Category, O => O.MapFrom(s => s.Category.Name))
                //.ForMember(d=>d.PictureUrl , O =>O.MapFrom(s => $"{"https://localhost:7124"}/{s.PictureUrl}" ))
                .ForMember(d => d.PictureUrl, O => O.MapFrom<ProductPitureUrlResolver>());

            CreateMap<CustomerBasketDto, CustomerBasket>();

            CreateMap<BasketItemDto, BasketItem>();

            CreateMap < AddressDto, Core.Entities.Order_Aggregate_Modul.Address>();
             
            CreateMap<Core.Entities.Identity.Address, AddressDto>().ReverseMap();

            CreateMap<Order, OrderToReturnDto>()
                .ForMember(d => d.DeliveryMethod, O => O.MapFrom(s => s.DeliveryMethod.ShortName))
                .ForMember(d=> d.DeliveryMethodCost , O=>O.MapFrom(s => s.DeliveryMethod.Cost));

            CreateMap<OrderItems, OrderItemDto>()
                .ForMember(d => d.ProductName, O => O.MapFrom(s => s.Product.ProductName))
                .ForMember(d => d.ProductId, O => O.MapFrom(s => s.Product.ProductId))
                .ForMember(d => d.PictureUrl, O => O.MapFrom(s => s.Product.PictureUrl))
                .ForMember(d => d.PictureUrl, O => O.MapFrom<OrderItemPictureUrlResolver>());
                ;

        }
    }
}
