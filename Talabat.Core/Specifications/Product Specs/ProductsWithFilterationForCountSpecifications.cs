using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities;

namespace Talabat.Core.Specifications.ProductsSpecs
{
    //count => Totaly of Data That was Exist before pagination 
    public  class ProductsWithFilterationForCountSpecifications:BaseSpecifications<Product>
    {
        public ProductsWithFilterationForCountSpecifications(ProductSpecParams specParams)
       : base(P =>
                   (string.IsNullOrEmpty(specParams.Search) || P.Name.ToLower().Contains(specParams.Search))&&

                   (!specParams.BrandId.HasValue || P.BrandId == specParams.BrandId.Value) &&
                   (!specParams.CategoryId.HasValue || P.CategoryId == specParams.CategoryId.Value)

         )

        {

        } 
    }
}
