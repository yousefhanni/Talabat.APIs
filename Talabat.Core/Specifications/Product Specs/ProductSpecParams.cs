using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Talabat.Core.Specifications.ProductsSpecs
{
    public class ProductSpecParams
    {
        private const int MaxPageSize = 10;   

        //Fullproperty => To Can Controle(Validate) at PageSize that front will send it
        private int pageSize=5; //5 => Default

        public int PageSize
        {
            get { return pageSize; }
            set { pageSize = value> MaxPageSize ? MaxPageSize : value; }
        }

        public int PageIndex { get; set; } = 1;

        private string? search;

        public string? Search
        {
            get { return search; }
            set { search = value?.ToLower(); }
        }


        public string? Sort { get; set; }

        public int? BrandId { get; set; }

        public int? CategoryId { get; set; }


    }
}
