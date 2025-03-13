using BussinessLayer.Interface;
using BussinessLayer.Service;
using Microsoft.AspNetCore.Mvc;
using ModelLayer.DTO;
using RepositoryLayer.Helper;

namespace HelloGreetingApplication.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {

        private readonly IUserBL _userBL;
        private readonly IEmailService _emailService;
        private readonly jwtHelper _jwtHelper;
       public UserController(IUserBL userBL, IEmailService emailService, jwtHelper jwtHelper)
        {
            _userBL = userBL;
            _jwtHelper = jwtHelper;  
            _emailService = emailService;
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
            string token = _userBL.Login(model);
            if (string.IsNullOrEmpty(token))
                return Unauthorized(new { error = "Invalid credentials" });

            return Ok(new { message = "Login successful!", token });
        }


        [HttpPost("ForgotPassword")]
        public IActionResult ForgotPassword([FromBody]  string email)
        {
            var user = _userBL.GetUserByEmail( email);
            if (user == null)
            {
                return BadRequest("User does not exist.");
            }

            string resetToken = _jwtHelper.GenerateResetToken(user);
            _userBL.SaveResetToken(user.Id, resetToken);



            string subject = "Password Reset Request";
            string body = $"https://V.com/resetpassword?token={resetToken}";
            

            bool emailSent = _emailService.SendEmail(user.Email, subject, body);
            if (!emailSent)
            {
                return StatusCode(500, "Error sending email.");
            }

            return Ok(new { Message = "Password reset link sent successfully." });

        }

        [HttpPost("ResetPassword")]
        public IActionResult ResetPassword([FromQuery] string token, [FromBody] ResetPasswordDTO model)
        {
            if (string.IsNullOrEmpty(token))
            {
                return BadRequest(new { message = "Token is required." });
            }

            var user = _userBL.ValidateResetToken(token);
            if (user == null)
            {
                return BadRequest(new { message = "Invalid or expired token." });
            }

            if (model.NewPassword != model.ConfirmPassword)
            {
                return BadRequest(new { message = "Passwords do not match." });
            }

            byte[] hashedPassword = PasswordHashing.HashPassword(model.NewPassword);
            _userBL.UpdatePassword(user.Id, hashedPassword);

            return Ok(new { message = "Password reset successfully." });
        }


    }

}

