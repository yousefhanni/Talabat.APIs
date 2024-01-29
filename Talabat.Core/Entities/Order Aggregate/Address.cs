using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Talabat.Core.Entities.Order_Aggregate_Modul
{
    ///About Address of order and user 
    ///Address of Order => Not Mapped to table at DB 
    ///Relationship between Order and address =>( One To One) and Total from Both sides =>
    ///Make Mapping => Put all Attributes at One Table(Order),Choose PK of any entity to this table
    ///Which => class address Not Mapped(represented) at Database
    ///How Obtains on Address of Order ? =>When user ask order =>take His address from table addresses
    ///And put it at table order as Address of order(address of user is Default address of order)
    ///And give to address of order Enablement to change it. 
    ///The address of Order is for one order only but The address of user is for one or more orders

    public class Address
    {
        //I Make this Constructor to enable EFCore to know use this class(to know Make Migrations) 
        public Address()
        {
            
        }


        public Address(string firstName, string lastName, string street, string city, string country)
        {
            FirstName = firstName;
            LastName = lastName;
            Street = street;
            City = city;
            Country = country;
        }

        public string  FirstName { get; set; }

        public string LastName { get; set; }

        public string Street { get; set; }

        public string City { get; set; }

        public string Country { get; set; }

    }
}
