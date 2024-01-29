using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation; 
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities.Order_Aggregate_Modul;

namespace Talabat.Repository.Data.Config
{
    internal class OrderConfigurations : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {
            ///Must define that address of order class will map at the same table that will map to it Order  
            ///Each Order OwnsOne =>ShippingAddress and ShippingAddress will happens to him Mapping WithOwner => Order
            builder.OwnsOne(O => O.ShippingAddress, ShippingAddress => ShippingAddress.WithOwner());//Mapping => 1:1[Total]

            //To store OrderStatus at DB as String and retrieve as OrderStatus[Enum]
            builder.Property(O => O.Status)
                
                .HasConversion(
                OStatus => OStatus.ToString(),

                OStatus => (OrderStatus)Enum.Parse(typeof(OrderStatus),OStatus) 
                );

            ///At which code from Two codes it will understand that relationship 1:1
            ///builder.HasOne(O => O.DeliveryMethod)
            /// .WithOne(); //Will make unique constrain on F.K
            ///OR 
            ///builder.HasIndex(O => O.DeliveryMethodId).IsUnique();

            builder.Property(O => O.Subtotal)
                    .HasColumnType("decimal(18,2)");


            ///onDelete By Default will is (Cascade) => Which if You Deleted DeliveryMethod
            ///Will Delete All Orders That belong to this DeliveryMethod.
            ///But I don't want that,I Want when delete DeliveryMethod 
            ///=>DeliveryMethodId that exist at All Orders is Null
            ///Which When delete DeliveryMethod =>Set DeliveryMethodId inside Orders With Null 
            ///How You want to DeliveryMethodId is Null and Datatype of DeliveryMethodId (is int Not Nullable int) ?
            ///There 3 Solutions :
            ///1.Write This F.K (public int? DeliveryMethodId { get; set; }) at class Order and Make it Nullable int
            ///OR
            ///2.Make Migaration then Change column (DeliveryMethodId)  at migration to nullable: true 
            ///and change it at Snapshot to (int?) to Change forever. 
            ///OR
            ///3. Make This property public (DeliveryMethod? DeliveryMethod { get; set; }) is nullable 

            builder.HasOne(O => O.DeliveryMethod)
                .WithMany()
                .OnDelete(DeleteBehavior.SetNull)
                ;


        }
    }
}
