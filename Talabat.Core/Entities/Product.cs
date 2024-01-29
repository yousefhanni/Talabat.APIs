using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Talabat.Core.Entities
{
    public class Product:BaseEntity
    {
        public string? Name { get; set; }
         
        public string? Description { get; set; }

        public string? PictureUrl { get; set; }   
         
        public decimal Price{ get; set;}

        //Fluent API 
        public int BrandId { get; set; } //Foreign Key Column --> ProductBrand

        public ProductBrand Brand { get; set; } //Navigational Property [ONE]


      //Fluent API 
        public int CategoryId { get; set; } //Foreign Key Column -->ProductCategory

        public ProductCategory Category { get; set; } //Navigational Property [ONE]

    }
}
