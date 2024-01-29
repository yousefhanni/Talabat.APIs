using AutoMapper;
using AutoMapper.Execution;
using Talabat.APIs.Dtos;
using Talabat.Core.Entities;

namespace Talabat.APIs.Helpers
{
    //This class that Will make Resolving (product url)
    public class ProductPitureUrlResolver : IValueResolver<Product, ProductToReturnDto, string?>
    {
        private readonly IConfiguration _configuration;

        public ProductPitureUrlResolver( IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public string Resolve(Product source, ProductToReturnDto destination, string destMember, ResolutionContext context)
        {
            if (!string.IsNullOrEmpty(source.PictureUrl))
                return $"{_configuration["ApiBaseUrl"]}/{source.PictureUrl}";

            return string.Empty ;

        }
    }
}
