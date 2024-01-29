using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Talabat.APIs.Dtos;
using Talabat.APIs.Errors;
using Talabat.APIs.Helpers;
using Talabat.Core.Entities;
using Talabat.Core.Repositories.Contract;
using Talabat.Core.Services.Contract;
using Talabat.Core.Specifications;
using Talabat.Core.Specifications.ProductsSpecs;

namespace Talabat.APIs.Controllers
{
    public class ProductsController : BaseApiController
    {
        //Make ProductsController deal with ProductService
        private readonly IProductService _productService;

        ///private readonly IGenericRepository<Product> _productsRepo;
        ///private readonly IGenericRepository<ProductBrand> _brandsRepo;
        ///private readonly IGenericRepository<ProductCategory> _categoriesRepo;
  
        private readonly IMapper _mapper;   

        //to take Object from IGenericRepository to can  GetProducts and GetProduct by id
        public ProductsController(
            IProductService productService,

                ///IGenericRepository<Product> productsRepo , 
                ///IGenericRepository<ProductBrand>brandsRepo,
                ///IGenericRepository<ProductCategory> categoriesRepo,
            
            IMapper mapper)
        {
            _productService = productService;

            ///_productsRepo = productsRepo;
            ///_brandsRepo = brandsRepo;
            ///_categoriesRepo = categoriesRepo;

            _mapper = mapper;
        }

        //*******This End point => Through it we can make( sorting ,Filteration ,Pagination and Searching )******//

       // [Authorize/*(AuthenticationSchemes =JwtBearerDefaults.AuthenticationScheme)*/] //Must Has token
        [HttpGet] // Get : /api/Products
        //(ProductSpecParams specParams) =>take object from ProductSpecParams To can receive More 3 params To Make Clean Code
        public async Task<ActionResult<Pagination<ProductToReturnDto>>> GetProducts([FromQuery]ProductSpecParams specParams)  
                 {    
            var products = await _productService.GetProductsAsync(specParams);
            var count = await  _productService.GetCountAsync(specParams) ;
            //Data after Filteration,sorting and Pagination
            var data = _mapper.Map<IReadOnlyList<Product>, IReadOnlyList<ProductToReturnDto>>(products);
            return Ok(new Pagination<ProductToReturnDto>(specParams.PageIndex, specParams.PageSize,count,data)); // to Return standard Response of Pagination to endpoint
                }
         
        //ProducesResponseType=> Define Shape Resonse at Swagger
        [ProducesResponseType(typeof(ProductToReturnDto),StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound) ]
        [HttpGet("{id}")]

        public async Task<ActionResult<ProductToReturnDto>> GetProduct(int id)
        {
       
            var product = await _productService.GetProductAsync(id);
           
            if(product == null)
                return NotFound(new ApiResponse(404)); //404

            return Ok(_mapper.Map<Product,ProductToReturnDto>(product)); //200
          
        }


        [HttpGet("brands")] //Get: /api/Products/brands
        public async Task <ActionResult<IReadOnlyList<ProductBrand>>> GetBrands()
        {
            var brands = await _productService.GetBrandsAsync();

            return Ok(brands);
        }


        [HttpGet("categories")] //Get: /api/Products/categories
        public async Task<ActionResult<IReadOnlyList<ProductCategory>>> Getcategories()
         {
            var categories = await _productService.GetCategoriesAsync();

            return Ok(categories);
        }

    }
}
