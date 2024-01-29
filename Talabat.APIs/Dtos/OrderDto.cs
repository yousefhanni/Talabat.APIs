using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using Talabat.APIs.Errors;
using Talabat.Core.Entities.Order_Aggregate_Modul;

namespace Talabat.APIs.Dtos
{
    public class OrderDto
    {
        [Required]
        public string BasketId { get; set; }

        [Required]
        [CustomNotNullOrZero]
        public int DeliveryMethodId { get; set; } 

        public AddressDto ShippingAddress { get; set; }
    }
}
