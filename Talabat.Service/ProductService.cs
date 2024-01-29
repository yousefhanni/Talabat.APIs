using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core;
using Talabat.Core.Entities;
using Talabat.Core.Services.Contract;
using Talabat.Core.Specifications.ProductsSpecs;

namespace Talabat.Service
{
    //Contain on ALL Methods(Logic) of Product Controller  
    public class ProductService : IProductService
    {
        private readonly IUnitOfWork _unitOfWork;
        //ProductService deal directly with unitOfWork  
        public ProductService(IUnitOfWork unitOfWork)
        {
           _unitOfWork = unitOfWork;
        }


        public async Task<IReadOnlyList<Product>> GetProductsAsync(ProductSpecParams specParams)
        {
            var spec = new ProductWithBrandAndCategorySpecifications(specParams);

            var products = await  _unitOfWork.Repository<Product>().GetAllWithSpecAsync(spec);

            return products;
        }

        public async Task<Product?> GetProductAsync(int productId)
        {
            var spec = new ProductWithBrandAndCategorySpecifications(productId);

            var product = await _unitOfWork.Repository<Product>().GetEntityWithSpecAsync(spec);

            return product;
        }

        public async Task<int> GetCountAsync(ProductSpecParams specParams)
        {
            /// spec => Specification of Query that will get Data
            ///countspec => Totaly of Data That was Exist before pagination 
           
            var countspec = new ProductsWithFilterationForCountSpecifications(specParams);
            var count = await _unitOfWork.Repository <Product>().GetCountAsync(countspec);

            return count;
        }


        public async Task<IReadOnlyList<ProductBrand>> GetBrandsAsync()
          => await _unitOfWork.Repository<ProductBrand>().GetAllAsync();
        

        public async Task<IReadOnlyList<ProductCategory>> GetCategoriesAsync()
         => await _unitOfWork.Repository<ProductCategory>().GetAllAsync();

    }
}
