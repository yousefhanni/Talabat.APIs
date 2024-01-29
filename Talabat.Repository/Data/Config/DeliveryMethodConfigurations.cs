using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities.Order_Aggregate_Modul;

namespace Talabat.Repository.Data.Config
{
    ///IEntityTypeConfiguration<T>: is an interface in Entity Framework Core that allows you
    ///to configure the mapping between an entity type(Class) (such as DeliveryMethod in your case)
    ///and the corresponding database table
    
    internal class DeliveryMethodConfigurations : IEntityTypeConfiguration<DeliveryMethod>
    {
        public void Configure(EntityTypeBuilder<DeliveryMethod> builder)
        {
            builder.Property(deliveryMethod => deliveryMethod.Cost)
                   .HasColumnType("decimal(18,2)");

     
        }
    }
}
