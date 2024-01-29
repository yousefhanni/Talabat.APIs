using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities.Order_Aggregate_Modul;

namespace Talabat.Core.Specifications.Order_Specs
{
    public class OrderSpecifications : BaseSpecifications<Order>  
    {
      ///Use This Constructor to Get Orders For Specific User 
      ///chaining on (BaseSpecifications Constructor) to Set Criteria(Where), Include Data at order,OrderByDesc
        public OrderSpecifications(string buyerEmail)
            :base(O => O.BuyerEmail==buyerEmail)
        {
            Includes.Add(O => O.DeliveryMethod);
            Includes.Add(O => O.Items);
            AddOrderByDesc(O => O.OrderDate); //Order(from The newest to Oldest ) based on OrderDate
        }
        ///Include(Loading) :
        ///Do order has (Navigational Property) need to make it eager loading(fetch data of entity with data of main entity(order) ) ?
        ///Do Address ? => No, Because Addresss exist at same table => 1:1 (T)
        ///Do DeliveryMethodId ? =>yes ,so,You want get(load) N.P(data of DeliveryMethod) at this order =>
        /// relationship = [1] , So Eager loading 
        ///Do items ? => yes , You want get(load) N.P(data of items) at this order => relationship = [M] =>
        ///Do Mandatory or optinal ? Mandatory => eager loading => because Must to get order must get items at same Query.  

        ///Order: 
        ///Do you need order by Dsc or Asc ? Yes,I need to get orders from new to old(Latest)  


        //Use This Constructor to Get Specific Order For Specific User 
        public OrderSpecifications(int orderId,string buyerEmail)
       : base(O => O.Id == orderId && O.BuyerEmail == buyerEmail)
        {
            Includes.Add(O => O.DeliveryMethod);
            Includes.Add(O => O.Items);
        }


        

    }
}
