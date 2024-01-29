using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities.Identity;

namespace Talabat.Core.Services.Contract
{
    //Signature for Method will Generate token 
    public interface IAuthService
    {

        Task<String> CreateTokenAsync(AppUser user,UserManager<AppUser>userManager); 

    }
}
