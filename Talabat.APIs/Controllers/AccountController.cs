using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Talabat.APIs.Dtos;
using Talabat.APIs.Errors;
using Talabat.APIs.Extensions;
using Talabat.Core.Entities.Identity;
using Talabat.Core.Services.Contract;

namespace Talabat.APIs.Controllers
{
    //Two End points(Login(signIn) + Register(SignUp))
    public class AccountController : BaseApiController
    {
        private readonly UserManager<AppUser>   _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly IAuthService _authService;
        private readonly IMapper _mapper;

        public AccountController(UserManager<AppUser> userManager,
            SignInManager<AppUser> signInManager,
           IAuthService authService,
           IMapper mapper
            
            )

        {
            _userManager = userManager;
            _signInManager = signInManager;
           _authService = authService;
           _mapper = mapper;
        }


        [HttpPost("login")] //Post: /api/account/login
        ///Dtos:
        ///Response(Return) object(UserDto) contain => Displayname + email + Token 
        ///Login take object(LoginDto) that contain => Email + Password
        public async Task<ActionResult<UserDto>> Login(LoginDto model)
        {
           var user = await _userManager.FindByEmailAsync(model.Email);
            //This Email Not Exist at DB 
            if(user is null)
                 return Unauthorized(new ApiResponse(401));

            var result = await _signInManager.CheckPasswordSignInAsync(user, model.Password,false);  
            
            if(result.Succeeded is false)
                return Unauthorized(new ApiResponse(401));

            // If pass and Email True will return object from class UserDto
            return Ok(new UserDto
            {
                DisplayName = user.DisplayName,
                Email = user.Email,
                Token = await _authService.CreateTokenAsync(user, _userManager)
            });
        }

        //Register => (Create Account)
        //registerDto=>Displayname + email + phoneNum + Password 
        [HttpPost("register")] //Post: /api/account/register
        public async Task<ActionResult<UserDto>> Register(RegisterDto model)
        {
            if (CheckEmailExist(model.Email).Result.Value)
                return BadRequest(new ApiValidationErrorResponse()
                { Errors =new string[] {"this email is already in user!!"} });

            //Object initialization 
            var user = new AppUser()
            {
                DisplayName = model.Displayname,      //DisplayName = yousef
                Email = model.Email,                  //yousef.hani@gmail.com
                UserName = model.Email.Split("@")[0], //yousef.hani
                PhoneNumber = model.PhoneNumber, 
            };
            //Create User and enter Data of user at Database  
            var result =await _userManager.CreateAsync(user,model.Password);

            if(result.Succeeded is false) return BadRequest(new ApiResponse(400));

            
             return Ok(new UserDto()
            {
              DisplayName=user.DisplayName,
              Email=user.Email,
              Token = await _authService.CreateTokenAsync(user, _userManager),
            });


        }

        //Get current User that sent request\made Login
        [Authorize]
        [HttpGet] //Get : /api/accounts
        public async Task<ActionResult<UserDto>> GetCurrentUser()
        {
            ///User: This property inside it Claims of Token and inherited from ControllerBase
            ///and ControllerBase that BaseApiController with inherit from it and
            ///BaseApiController that AccountController with inherit from it

        var Email = User.FindFirstValue(ClaimTypes.Email); //Get Email of user that sent request
        var user = await _userManager.FindByEmailAsync(Email); //Get User that sent request by Email
            return Ok(new UserDto()
            {
                DisplayName=user.DisplayName,

                Email=user.Email,

                Token =  await  _authService.CreateTokenAsync(user,_userManager)

            }); 
        }

        [Authorize]
        [HttpGet("address")] //Get : /api/Account/address

        public async Task<ActionResult<AddressDto>> GetUserAddress()
         {
            var user = await _userManager.FindUserWithAddressAsync(User);
             
            var address= _mapper.Map<AddressDto>(user.Address);

            return Ok(address);

        }

        [Authorize]
        [HttpPut("address")] //PUT : /api/accounts/address

        public async Task<ActionResult<AddressDto>>UpdateUserAddress(AddressDto updatedAddress)
        {
            var address = _mapper.Map<AddressDto,Address>(updatedAddress);

            var user = await _userManager.FindUserWithAddressAsync(User); 

            address.Id=user.Address.Id;
            user.Address=address; //change object state of address to is Modified

            var result=await _userManager.UpdateAsync(user);                

            if(!result.Succeeded)
                return BadRequest(new ApiResponse(400));

            return Ok(updatedAddress);

        }


        //To avoid any person that with try to register that write Email exist

        [HttpGet("emailexists")] //Get : /api/account/emailexists?email=yousef.hani@gamil.com

        public async Task<ActionResult<bool>>CheckEmailExist(string email)
        {
            return await _userManager.FindByEmailAsync(email) is not null;  

        }




    }
}
