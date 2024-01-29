using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;
using Talabat.APIs.Errors;
using Talabat.APIs.Helpers;
using Talabat.Core;
using Talabat.Core.Repositories.Contract;
using Talabat.Core.Services.Contract;
using Talabat.Repository;
using Talabat.Service;

namespace Talabat.APIs.Extensions
{
    public static class ApplicationServicesExtension
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {

            services.AddScoped(typeof(IPaymentService), typeof(PaymentService));

            services.AddScoped(typeof(IProductService), typeof(ProductService));


            ///Allow DI To OrderService => when any body ask(Inject) object from type (IOrderService)
            ///Create object from type class (OrderService) 

            services.AddScoped(typeof(IOrderService), typeof(OrderService));


            ///Allow DI To UnitOfWork => when any body ask object from type (IUnitOfWork)
            ///Create object from type class (UnitOfWork) 

            services.AddScoped(typeof(IUnitOfWork),typeof(UnitOfWork));

            //Allow DI To BasketRepository 
            services.AddScoped(typeof(IBasketRepository), typeof(BasketRepository));

        
            //services.AddScoped<IGenericRepository<Product>,GenericRepository<Product>>();
            //services.AddScoped<IGenericRepository<ProductBrand>, GenericRepository<ProductBrand>>();
            //services.AddScoped<IGenericRepository<ProductCategory>, GenericRepository<ProductCategory>>();

            //When ask to create object from type IGenericRepository of thing => create object of type GenericRepository of this thing(instead of 3 lines previous  ) 
           /* services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));*/ //Allow DI  

            services.AddAutoMapper(typeof(MappingProfiles));     //profile => convert from type to another 



            //**/This Configuration To validation Error is created only once per the project //**/ 

            //Resposible for Endpoint has validation Error

            services.Configure<ApiBehaviorOptions>(Options =>
            {
                //there Factory is responsible for Generation of validation error to Invalid model state 

                //actionContext =>Context of action that Resposible for Endpoint has Invalid model state 
                Options.InvalidModelStateResponseFactory = (actionContext) =>
                {
                    ///Change Factory that Resposible for Endpoint has Invalid model state 
                    ///I want to get on dictionary that has key value pair to all model(parameter)
                    ///,which get parameters that have errors(that have state is not valid) 
                    ///Key => parameter , value => Array of errors that has parameter

                    var errors = actionContext.ModelState.Where(P => P.Value.Errors.Count() > 0)
                                                        .SelectMany(P => P.Value.Errors)   //To Put arrays of error At one array of errors 
                                                        .Select(E => E.ErrorMessage) //To select from all error message
                                                        .ToArray(); //Get all errors at one array 

                    var validationErrorResponse = new ApiValidationErrorResponse()
                    {
                        Errors = errors
                    };

                    return new BadRequestObjectResult(validationErrorResponse);
                };

            });

            return services;
        } 
    }
}
