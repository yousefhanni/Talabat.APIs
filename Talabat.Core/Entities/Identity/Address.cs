namespace Talabat.Core.Entities.Identity
{
    //Address of User => Table at Database 
    public class Address
    {
        public int Id { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Street { get; set; }
        
        public string City { get; set; }

        public string AppUserId { get; set; }//Foreign Key :Users  

        public AppUser User { get; set; } //N.P[ONE]

        public string Country { get; set; }

    }
}