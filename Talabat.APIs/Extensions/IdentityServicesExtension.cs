using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Talabat.Core.Entities.Identity;
using Talabat.Core.Services.Contract;
using Talabat.Repository.Identity;
using Microsoft.IdentityModel.Tokens;
using Talabat.Service;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace Talabat.APIs.Extensions
{
    public static class IdentityServicesExtension
    {
        //Extension Method to IServiceCollection
        public static IServiceCollection AddIdentityServices(this IServiceCollection services , IConfiguration configuration)
        {
            ///Allow DI To AuthService => when any body ask(Inject) object from type (IAuthService)=>
            ///Create object from type class (AuthService) 
            
            services.AddScoped(typeof(IAuthService), typeof(AuthService));

            //Allow DI To Identity Services(usermanager , SignInManager,RoleManager)
           services.AddIdentity<AppUser, IdentityRole>(options =>
            {
                ///options.Password.RequiredUniqueChars = 2;
                ///options.Password.RequireNonAlphanumeric = true;
                ///options.Password.RequireUppercase= true;
                ///options.Password.RequireLowercase= true;
                
            }).AddEntityFrameworkStores<AppIdentityDbContext>();//(Stores == Repositories)
            ///(AddEntityFrameworkStores) => Allow DI to Stores(CreateUser\Role,Updateuser\Role,Finduser,...)that need it the (Identity Services) ,
            ///Cycle ==> The Account controller will depend on object from (Usermanager service)
            ///and (Usermanager service) depend on object from (Stores) and Store depend on object from AppIdentityDbContext

            
            ///telling the DI container to set up and make available the necessary services that handle authentication tasks.
            ///Add authentication handler that Validate(handle) Token
            
            services.AddAuthentication(/*JwtBearerDefaults.AuthenticationScheme*/ options =>
            {
                //To define Name of Schema
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                //To Put default Scheme (Bearer) on any endpoint is Authorized
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }) 
                .AddJwtBearer(options => {       

                    //Configure Authentication handler 
                    //Which Validate on Token 
                    options.TokenValidationParameters = new TokenValidationParameters()
                    {
     
                        ValidateAudience = true,    
                        ValidAudience = configuration["JWT:ValidAudience"],
                        ValidateIssuer = true,
                        ValidIssuer= configuration["JWT:ValidIssuer"],
                        ValidateIssuerSigningKey=true, 
                        IssuerSigningKey= new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JWT:SecretKey"])),
                        ValidateLifetime = true,
                        ClockSkew =TimeSpan.FromDays(double.Parse(configuration["JWT:DurationInDays"]))

                };

                }).AddJwtBearer("Bearer02", options =>
                {

                })
                .AddCookie("BBB",options =>  //Default Schema is named (Cookies)
                {


                });
                                         

      
            return services;  
        }
    }
}
