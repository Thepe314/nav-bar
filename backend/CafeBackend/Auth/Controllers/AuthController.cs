using CafeBackend.Auth.Dtos;
using CafeBackend.Auth.Models;
using CafeBackend.Auth.Repositories;
using CafeBackend.Auth.Service;
using Microsoft.AspNetCore.Mvc;

namespace CafeBackend.Auth.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        //Readonly
        private readonly IAuthRepository _authRepository;

        private readonly TokenService _tokenService;

        public AuthController(IAuthRepository authRepository, TokenService tokenService)
        {
            _authRepository = authRepository;

            _tokenService = tokenService;
        }

        //Endpoints
        
        //Signup
        [HttpPost("Signup")]
        public async Task<IActionResult> Signup( SignupDto dto)
        {
           //check email if it exist
            var email = dto.Email.Trim().ToLower();
            var existing = await _authRepository.GetbyEmail(email);
            if(existing != null)
            {
                return BadRequest("Email already Exist");
            }

            var user = new User
            {
                FullName = dto.FullName,
                Email = dto.Email,
                Password = BCrypt.Net.BCrypt.HashPassword(dto.Password),
                Role = "User"
            };

            await _authRepository.CreateUser(user);
           
            return Ok("Successfully Signed Up");
        }

        //Login
        [HttpPost("Login")]
        public async Task<IActionResult> Login(LoginDto dto)
        {
            //Check if User exists
            var email = dto.Email.Trim().ToLower();
            var existing = await _authRepository.GetbyEmail(email);
            if(existing == null)
            {
                Unauthorized("Invalid Email");
            }

            //verify password 
            bool VerifyPassword = BCrypt.Net.BCrypt.Verify(dto.Password,existing.Password);

            if(!VerifyPassword)
            {
                return Unauthorized("Invalid Password");
            }

            //Later generate jwt
            var token = _tokenService.GenerateToken(existing);

    
            return Ok(new{Message = "Login in successfully",Email = existing.Email, Jwt=token});
        }
        
        
    } 

}
