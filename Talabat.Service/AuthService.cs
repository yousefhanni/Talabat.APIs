using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities.Identity;
using Talabat.Core.Services.Contract;

namespace Talabat.Service
{
    //Auth Service is a Common service and repeat at All Projects 
    //The first You Must to Install Package JWT 

    public class AuthService : IAuthService
    {
        private readonly IConfiguration _configuration;

        public AuthService(IConfiguration configuration)
        {
           _configuration = configuration;
        }
        public async Task<string> CreateTokenAsync(AppUser user, UserManager<AppUser> userManager)
        {
            //Payload of Token :
            //Private Claims (User-Defined)

            var authClaims = new List<Claim>()
            {
                new Claim(ClaimTypes.GivenName,user.UserName),
                new Claim(ClaimTypes.Email,user.Email)
            };

            ///May use Roles as private Claims 
            ///if front want to Know what is role of user(Manager,employee,customer,...) 
            ///To Put roles of users at Token

            var userRoles = await userManager.GetRolesAsync(user);

            foreach (var role in userRoles)
                authClaims.Add(new Claim(ClaimTypes.Role, role));


            //Generate Secret Key to Make encoding to (Header and payload):
            var authkey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:SecretKey"])); //put SecretKey at appsettings

            //token: token object use it to build token
            var token = new JwtSecurityToken(
                ///claims : 
                ///Registered claims: These are a set of predefined claims which are not mandatory but recommended,
                ///to provide a set of useful, interoperable claims. Some of them are: iss (issuer),
                ///exp (expiration time), sub (subject), aud (audience)
                ///OR
                ///Private claims: These are the custom claims created to share information between parties

                //1.Put Registered claims
                audience: _configuration["JWT:ValidAudience"],
                issuer: _configuration["JWT:ValidIssuer"],
                expires: DateTime.UtcNow.AddDays(double.Parse(_configuration["JWT:DurationInDays"])),
                //2.Put Private claims
                claims: authClaims,

                //3.Send Secret key and security Algo
                signingCredentials: new SigningCredentials(authkey,SecurityAlgorithms.HmacSha256Signature)

                );

            //return Token iteself
            return  new JwtSecurityTokenHandler().WriteToken(token);




        }



    }
}

///Authentication is the process of determining a user's identity.
///Authorization is the process of determining whether a user has access to a resource.(Role of user)
///if not exist any Roles => Authentication is Authorization(if it Authenticated so it Authorized )
//Authorized => Has token
//The authentication scheme can select which authentication handler is responsible for generating the correct set of claims