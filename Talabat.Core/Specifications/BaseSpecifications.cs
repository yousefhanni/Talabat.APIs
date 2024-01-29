using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities;

namespace Talabat.Core.Specifications
{ 
     ///This class is designed to be a base class for creating specifications
     ///that can be used for querying data with criteria, includes, ordering, and pagination

    public class BaseSpecifications<T> : ISpecifications<T> where T : BaseEntity
    {
        //Used for filtering data.
        public Expression<Func<T, bool>> Criteria { get; set; } = null;

        ///The Includes property is a list of include expressions,
        ///indicating related entities that should be eagerly loaded with the main entity.
        ///This helps to avoid the N+1 query problem when fetching related data.

        //Initialize Includes => Empty List
        public List<Expression<Func<T,object>>> Includes { get; set ; }=new List<Expression<Func<T, object>>>();                                                                                                                     //
        //n for sorting data in ascending order.
        public Expression<Func<T, object>> OrderBy { get; set; } = null;
        //for sorting data in descending order.
        public Expression<Func<T, object>> OrderByDesc { get; set; } = null;

        //Represents the number of items to skip when applying pagination.
        public int Skip { get; set; }
        // Represents the number of items to take when applying pagination.
        public int Take { get; set; }
   
        public bool IsPaginationEnabled { get; set; }

        ///when create object form this Cons, new keyword will allocate object + initial value of Propery(null) and excute Cons,
        ///but must be (Includes) are not null, must point on object at heap to can put inside it expressions that you want to be included(brand,category)       
        ///must be initialization to Includes => To Can Add inside list 

        //Use cons to Create object from BaseSpecifications and Make Criteria is null and build Query Get all Products 
        public BaseSpecifications()
        {
            // Criteria = null;
        }

        //Use cons to Create object from BaseSpecifications and Make Criteria is value and build Query Get Specific Product

        public BaseSpecifications( Expression<Func<T, bool>> criteriaExpression)
        {
            Criteria= criteriaExpression;  //O => O.BuyerEmail==buyerEmaill
        }


        //Two Methods  Act as Setter to OrderBy/Desc 
        public void AddOrderBy(Expression<Func<T, object>> orderByExpression) 
        { 
          OrderBy = orderByExpression; //P => P.Name
        }

        public void AddOrderByDesc(Expression<Func<T, object>> OrderByDescExpression)
        {
            OrderByDesc = OrderByDescExpression;
        }

        //To Apply Pagination must be call Method 

        public void ApplyPagination(int skip ,int take)
        {
            IsPaginationEnabled=true;

            Skip=skip;

            Take=take;  
        }
    }
}
