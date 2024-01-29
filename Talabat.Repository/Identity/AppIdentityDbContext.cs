using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities.Identity;

namespace Talabat.Repository.Identity
{
    
   public class AppIdentityDbContext:IdentityDbContext<AppUser>
    {
        ///Open Connection with Data base and Create DB named "Talabat.APIs.Identity"
        ///Creation object from AppIdentityDbContext depend on object from DbContextOptions<AppIdentityDbContext>
        ///that is named (options),when allow DI to (AppIdentityDbContext) need to
        ///Configure on (options) with "connectionString" instead of override OnConfiguring method 
        ///
        public AppIdentityDbContext(DbContextOptions<AppIdentityDbContext> options)
            : base(options)
        {
            
        }

        ///If you doesnt DBset<> to addresss class , Do will convert to table ?
        ///Yes,Because address class on relationship with AppUser class ,
        ///EFCore will understand by default that address is table.



        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<Address>().ToTable("Addresses");
        }

    }
}
