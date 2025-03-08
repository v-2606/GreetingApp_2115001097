using BussinessLayer.Interface;
using Microsoft.AspNetCore.Mvc;
using ModelLayer.DTO;

namespace HelloGreetingApplication.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class UserController:ControllerBase
    {
        
            private readonly IUserBL _userBL;

            public UserController(IUserBL userBL)
            {
                _userBL = userBL;
            }

        [HttpPost("register")]
        public IActionResult Register([FromBody] RegisterDTO model)
        {
            if (model == null)
                return BadRequest(new { error = "Invalid request data" });

            try
            {
                bool result = _userBL.Register(model);
                if (result)
                    return Ok(new { message = "User registered successfully!" });

                return BadRequest(new { error = "Registration failed" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = "Internal Server Error", details = ex.Message });
            }
        }

        [HttpPost("login")]
            public IActionResult Login([FromBody] LoginDTO model)
            {
                bool result = _userBL.Login(model);
                if (result)
                    return Ok(new { message = "Login successful!" });
                return Unauthorized(new { error = "Invalid credentials" });
            }
        }


    }

