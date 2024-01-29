using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities;
using Talabat.Core.Entities.Order_Aggregate_Modul;

namespace Talabat.Repository.Data
{
    public class StoreContext:DbContext
    {

        public StoreContext(DbContextOptions<StoreContext> options) :base(options)
        {
            
        }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
         ///Entity Framework Core will search for all classes in the executing assembly that implement
         ///the IEntityTypeConfiguration < T > interface and automatically apply their configurations at DB.
         
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }
       ///About DBSet<>
       ///Entities will Mapping to Database Tables withoutu using DbSet<> because i use (IEntityTypeConfiguration<T>) 
       ///But I Wrote this DBsets<> becuase any body will read My Code know Classes Represented as Tables at DB
      
        public DbSet<Product> Products { get; set; }

        public DbSet<ProductBrand> ProductBrands { get; set; }

        public DbSet<ProductCategory> ProductCategories { get; set; }

        public DbSet<Order> Order { get; set; }

        public DbSet<OrderItems> OrderItems { get; set; }

        public DbSet<DeliveryMethod> DeliveryMethod { get; set; }




    }
}
