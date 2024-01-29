using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities;

namespace Talabat.Core.Specifications.ProductsSpecs
{
    //This class will Add values to Brand and Category (include to Brand and Category with get all and get by id) 
    public class ProductWithBrandAndCategorySpecifications : BaseSpecifications<Product>
    {
        //Include was at (BaseSpecifications) Empty list which Not Included to Brand and Category
    
        //This Constructor will be used for Creating an Object,That Will be used to Get All products
        public ProductWithBrandAndCategorySpecifications(ProductSpecParams specParams)
            : base(P =>

                   (string.IsNullOrEmpty(specParams.Search) || P.Name.ToLower().Contains(specParams.Search))&&
                   //To Make Filteration
                   (!specParams.BrandId.HasValue    || P.BrandId == specParams.BrandId.Value)  &&
                   (!specParams.CategoryId.HasValue || P.CategoryId == specParams.CategoryId.Value)
                 )
        {
            //Includes is list carry Two includes expression and Criteria is null
            Includes.Add(P => P.Brand);
            Includes.Add(P => P.Category);


            if (!string.IsNullOrEmpty(specParams.Sort))
            {
                switch (specParams.Sort)
                {
                    case "priceAsc":
                        //OrderBy = P=>P.Price;   
                        AddOrderBy(P => P.Price);
                        break;

                    case "priceDesc":
                        //OrderByDesc = P => P.Price;
                        AddOrderByDesc(P => P.Price);
                        break;

                  default:        
                            AddOrderBy(P => P.Name);
                        break;
                }

            }

            else      

              AddOrderBy(P => P.Name);


            //totalproducts = 18 ~ 20
            //PageSize      = 5
            //PageIndex     = 3

            //ApplyPagination(Skip(10),Take(5)) , => Skip() will convert in sql to OFFSET() and Take() will convert to FETCH() 
            ApplyPagination((specParams.PageIndex - 1) * specParams.PageSize, specParams.PageSize);
        }

        //This Constructor will be used for Creating an Object,That Will be used to Get a Specific product with by id
        public ProductWithBrandAndCategorySpecifications(int id)  
            :base(P => P.Id==id)
        {
            Includes.Add(P => P.Brand);
            Includes.Add(P => P.Category);
        }



    }
}
