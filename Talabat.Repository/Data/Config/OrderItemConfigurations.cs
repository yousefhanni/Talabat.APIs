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
    internal class OrderItemConfigurations : IEntityTypeConfiguration<OrderItems>
    {
        public void Configure(EntityTypeBuilder<OrderItems> builder)
        {
            ///Must define that ProductItemOrdered class will mapping at the same table that will map to it OrderItem  
            ///Each OrderItem OwnsOne =>one Product and Product will happens to him Mapping WithOwner => OrderItem
            builder.OwnsOne(OrderItem => OrderItem.Product, Product => Product.WithOwner());

            builder.Property(OrderItem => OrderItem.Price)
                   .HasColumnType("decimal(18,2)");
        }
    }
}

