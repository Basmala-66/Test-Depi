using E_commerce_System.DTOs.AccountDTOs;
using E_commerce_System.Entities;
using E_commerce_System.services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace E_commerce_System.DTOs.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IJwtService _jwtService;

        public AccountController(UserManager<ApplicationUser> userManager,SignInManager<ApplicationUser> signInManager,IJwtService jwtService)
        {
            _userManager = userManager;
           _signInManager = signInManager;
            _jwtService = jwtService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(LoginDto request)
        {
            // auto mapper ممكن اعمل ريجييتريشن ب 
            var user = new ApplicationUser
            {
                UserName= request.email,
                Email= request.email,
            };

            var result = await _userManager.CreateAsync(user, request.password);
       
            if(!result.Succeeded)
            {
                return BadRequest(result.Errors);
            }
            return Created();
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDto request)
        {
            var user = await _userManager.FindByEmailAsync(request.email);
            if (user is null)
                return Unauthorized();

            var result = _signInManager.CheckPasswordSignInAsync(user,request.password,false);


            var token = _jwtService.GenerateToken(user);

            return Ok(token);


        }
    }
}
