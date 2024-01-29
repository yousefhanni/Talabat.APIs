using Microsoft.AspNetCore.Connections;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using StackExchange.Redis;
using Talabat.APIs.Errors;
using Talabat.APIs.Extensions;
using Talabat.APIs.Helpers;
using Talabat.APIs.MiddleWares;
using Talabat.Core.Entities;
using Talabat.Core.Entities.Identity;
using Talabat.Core.Repositories.Contract;
using Talabat.Core.Services.Contract;
using Talabat.Repository;
using Talabat.Repository.Data;
using Talabat.Repository.Identity;
using Talabat.Service;
using static System.Net.Mime.MediaTypeNames;

namespace Talabat.APIs
{
    public class Program
    {
        //Entry Point
        public static async Task Main(string[] args)
        {

            var WebApplicationbuilder = WebApplication.CreateBuilder(args);


            #region Configure Services
            //DI container is responsible for managing and providing instances of services throughout the application.

            // Add services to the container.

            WebApplicationbuilder.Services.AddControllers(); //Add APIs Services to The DI Container 

            WebApplicationbuilder.Services.AddSwaggerServices(); //Call extension Function From user defined class(SwaggerServicesExtension)

            //Allow (DI) to Dbcontext => Link between PL Layer(Talabat.APIs) and  Repository Layer
            //Because I [installed package of Dbcontext at Repository] + [to can  Call StoreContext]

            WebApplicationbuilder.Services.AddDbContext<StoreContext>(options =>
            {
                options.UseSqlServer(WebApplicationbuilder.Configuration.GetConnectionString("DefaultConnection"));
            });

            WebApplicationbuilder.Services.AddDbContext<AppIdentityDbContext>(optionsBuilder =>
            {
                optionsBuilder.UseSqlServer(WebApplicationbuilder.Configuration.GetConnectionString("IdentityConnection"));
            });

            //Allow (DI) to Redis DB
            //why AddSingleton ? to 1.once connection opened remains present 2. To caching 
            WebApplicationbuilder.Services.AddSingleton<IConnectionMultiplexer>((serviceProvider) =>
            {
                var connection = WebApplicationbuilder.Configuration.GetConnectionString("Redis");
                return ConnectionMultiplexer.Connect(connection);
            });


            //Call extension Function Contain on Some services,Function exists at user defined class(ApplicationServicesExtension)
            WebApplicationbuilder.Services.AddApplicationServices();

            WebApplicationbuilder.Services.AddIdentityServices(WebApplicationbuilder.Configuration);

            WebApplicationbuilder.Services.AddCors(options=>
            {
                options.AddPolicy("MyPolicy", options =>
                {
                    options.AllowAnyHeader().AllowAnyMethod().WithOrigins(WebApplicationbuilder.Configuration["FrontBaseUrl"]);
                });
            });
            #endregion

            var app = WebApplicationbuilder.Build();


            #region  Update-Database
            
            //When run project => Make Update Database from itself
            //(befor running check if there any Migrations not Applied at DB => make Apply from itself)
            //And if All Migrations Applied Will Send Message for you that all Migrations already Updated 

            using var Scope = app.Services.CreateScope(); //Step01, To Can With this Scope Can Ask object from any Serice

            var Services = Scope.ServiceProvider;//Step02, Used to Ask Service from scope 

            var _dbContext = Services.GetRequiredService<StoreContext>(); //Step03, Ask CLR for Creating Object from DbContext Explicitly  

            var _identityDbContext=Services.GetRequiredService<AppIdentityDbContext>();//Take Object from DbContext To Security

            var loggerFactory = Services.GetRequiredService<ILoggerFactory>(); //color the error and show specific Text  

            try
            {
            await _dbContext.Database.MigrateAsync(); //Step04 , ,Update-Database
            await StoreContextSeed.SeedAnsync(_dbContext); //Data Seeding

             await _identityDbContext.Database.MigrateAsync();//Update-Database To Security

                //Ask CLR Create object from class (UserManager) Explicitly
                var _userManager = Services.GetRequiredService<UserManager<AppUser>>();
                await AppIdentityDbContextSeed.SeedUsersAsync(_userManager);
            }

            catch (Exception ex)
            {
                var logger = loggerFactory.CreateLogger<Program>();
                logger.LogError(ex, "an error has occured during apply the migration");
           
            }

            //What are benifits from create Object from DbContext Explicitly =>
            //1. Automatic Migration
            //2.Data Seeding 
            #endregion



            #region Configure Kestrel Middlewares
            //This Middleware is Common,is created only once per the project 
            //Will Execute Invoke Function 
            app.UseMiddleware<ExceptionMiddleware>();


            // Configure the HTTP request pipeline.(Middleware)
            if (app.Environment.IsDevelopment())
            {
                app.UseSwaggerMiddleWares();  //Call extension Function From user defined class(SwaggerServicesExtension)
            }
          
            app.UseStatusCodePagesWithRedirects("/errors/{0}"); //0 =>code,Redirect to Specific Endpoint

            app.UseHttpsRedirection();
            
            app.UseStaticFiles(); //to can use files that inside(WWWroot(images))

            app.UseCors("MyPolicy"); 

            app.MapControllers();  //Read Routing Exist Per Controller

            app.UseAuthentication(); //Check if Token Exist or Not and if Exist it Valid or not

            app.UseAuthorization(); //Must that is has role 


            #endregion

            app.Run();
        }
    }
}