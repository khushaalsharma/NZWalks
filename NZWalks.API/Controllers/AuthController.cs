using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using NZWalks.API.Models.DTOs;
using NZWalks.API.Repositories;

namespace NZWalks.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<IdentityUser> userManager;
        private readonly ITokenRepository tokenRepository;

        public AuthController(UserManager<IdentityUser> userManager, ITokenRepository tokenRepository)
        {
            this.userManager = userManager;
            this.tokenRepository = tokenRepository;
        }


        //register method
        [HttpPost]
        [Route("Register")]
        public async Task<IActionResult> register([FromBody] RegisterRequestDto registerRequestDto)
        {
            var identityUser = new IdentityUser
            {
                UserName = registerRequestDto.username,
                Email = registerRequestDto.username,

            };

            var identityResult = await userManager.CreateAsync(identityUser, registerRequestDto.password);

            if (identityResult.Succeeded) //success or failure in registering
            {
                //Add roles to this user
                if (registerRequestDto.Roles != null && registerRequestDto.Roles.Any())
                {
                    identityResult = await userManager.AddToRolesAsync(identityUser, registerRequestDto.Roles);

                    if (identityResult.Succeeded)//confirms full registration
                    {
                        return Ok("user was registered");
                    }
                }
            }

            return BadRequest("Something went Wrong");
        }

        //Login method

        [HttpPost]
        [Route("Login")]

        public async Task<IActionResult> login([FromBody] LoginRequestDto loginRequestDto)
        {
            var user = await userManager.FindByEmailAsync(loginRequestDto.Username);

            if(user == null)
            {
                return BadRequest("Username or Password incorrect");
            }

            var isValid = await userManager.CheckPasswordAsync(user, loginRequestDto.Password);
            if(isValid == false)
            {
                return BadRequest("Wrong Password");
            }
            else
            {
                //getting roles for JWT token
                var roles = await userManager.GetRolesAsync(user);
                if(roles != null)
                {
                    var token = tokenRepository.CreateJWTToken(user, roles.ToList());
                    var response = new LoginResponseDto
                    {
                        JwtToken = token,
                    };

                    return Ok(response);
                }
            }
            return BadRequest(); 
        }
    }
}
