using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Talabat.Core.Entities.Identity
{
    public class AppUser:IdentityUser
    {
        //Customize on properties of Users
        public string DisplayName { get; set; }

        public Address Address { get; set; } //N.P[One]
    }
}
