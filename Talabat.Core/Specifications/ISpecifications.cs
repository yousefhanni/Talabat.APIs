using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities;

namespace Talabat.Core.Specifications
{   //This interface that define Shape Specification object(Parameter of Method)
    //that i will send it to Method(Dbset,Object from class that implement this interface) 
    //Specification => Query that will run against DBset<T> Which (T) must be Entity
    //(where and include) =>Two Specifications
    //Inside This interface Make=>Property signatures for each Specification and each Property carry value of Spec(where or inlcude or etc..)
    //And Type of value (where or inlcude ) is Lamda Expression
    public interface ISpecifications<T> where T : BaseEntity
    {
        //Criteria=> that carry value that will be sent to (Where)
        public Expression<Func<T,bool>> Criteria { get; set; }  //P=> P.Id==1

        //there More than (include) => so, i will use list 

        public List<Expression<Func<T,object>>> Includes { get; set; }

        //Two new Properties of Sorting 
        public Expression<Func<T,object>> OrderBy { get; set; }

        public Expression<Func<T, object>> OrderByDesc { get; set; }

        public int Skip { get; set; }

        public int Take { get; set; }

        //If true => Make skip and Take,based on this property define if Apply Pagination or not
        public bool IsPaginationEnabled { get; set; }

    }
}
