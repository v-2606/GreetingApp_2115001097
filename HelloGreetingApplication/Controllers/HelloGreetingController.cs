//using System.IdentityModel.Tokens.Jwt;
//using BussinessLayer;
//using BussinessLayer.Interface;
//using BussinessLayer.Service;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.EntityFrameworkCore;
//using Middleware;
//using ModelLayer.Model;
//using RepositoryLayer.Context;
//using RepositoryLayer.Entity;
//using RepositoryLayer.Helper;

//namespace HelloGreetingApplication.Controllers
//{

//    /// <summary>
//    /// class providing API for HelloGreeting
//    /// </summary>


//    [ApiController]
//    [Route("[controller]")]
//    public class HelloGreetingController : ControllerBase

//    {
//        private readonly ILogger<HelloGreetingController> _logger;
//        private readonly IGreetingService _greetingService;
//        private readonly UserContext _context;
//        private readonly JwtHelper _jwtHelper;


//        /// <summary>
//        /// Constructor to initialize logger.
//        /// </summary>
//        public HelloGreetingController(ILogger<HelloGreetingController> logger, IGreetingService greetingService, UserContext context, JwtHelper jwtHelper)
//        {
//            _logger = logger;
//            _greetingService = greetingService;
//            _context = context ?? throw new ArgumentNullException(nameof(context));
//            _jwtHelper = jwtHelper;
//        }

//        /// <summary>
//        /// Get Method to get the greeting message
//        /// </summary>
//        /// <returns> Hello World!</returns>
//        /// 

//        [HttpGet("getGreet")]
//        public IActionResult GetGreetings()
//        {

//            try
//            {
//                var greetings = _context.Set<GreetingEntity>().ToList();

//                return Ok(new { Success = true, Data = greetings });
//            }

//            catch (Exception ex)
//            {
//                var errorResponse = ExceptionHandler.CreateErrorResponse(ex, _logger);
//                return StatusCode(500, errorResponse);


//            }
//        }
//        [HttpPost("getToken")]
//        public IActionResult GetToken()
//        {
//            try {
//                string token = _jwtHelper.GenerateToken(1, "Varsha", "varsha@example.com", "Pass@123"); // Example data
//                return Ok(new { Token = token });
//            }
//            catch (Exception ex)
//            {
//                var errorResponse = ExceptionHandler.CreateErrorResponse(ex, _logger);
//                return StatusCode(500, errorResponse);


//            }

//        }
//        [HttpGet("validateToken")]
//        public IActionResult ValidateToken([FromBody] TokenRequestcs tokenRequest)
//        {

//            try
//            {
//                var claims = _jwtHelper.ValidateToken(tokenRequest.Token);
//                if (claims == null)
//                {
//                    return Unauthorized("Invalid token");
//                }

//                return Ok(new { Message = "Token is valid", Claims = claims.Claims.Select(c => new { c.Type, c.Value }) });
//            }

//            catch (Exception ex)
//            {
//                var errorResponse = ExceptionHandler.CreateErrorResponse(ex, _logger);
//                return StatusCode(500, errorResponse);


//            }
//        }

//        [HttpGet]
//        public IActionResult Get(string? firstName = "", string? lastName = "")
//        {
//            try {
//                _logger.LogInformation("GET request received.");
//                string message = _greetingService.GetGreetingMessage(firstName, lastName);
//                ResponseModel<string> responseModel = new ResponseModel<string>();
//                responseModel.Success = true;
//                responseModel.Message = " Hello to Greeting App API EndPoint";
//                responseModel.Data = message;
//                return Ok(responseModel);
//            }

//            catch (Exception ex)
//            {
//                var errorResponse = ExceptionHandler.CreateErrorResponse(ex, _logger);
//                return StatusCode(500, errorResponse);


//            }
//        }

//        [HttpPost]
//        public IActionResult Post(RequestModel requestModel)
//        {

//            try {
//                if (string.IsNullOrEmpty(requestModel?.Key) || string.IsNullOrEmpty(requestModel?.Value))
//                {
//                    return BadRequest(new ResponseModel<string>
//                    {
//                        Success = false,
//                        Message = "Invalid request. Key and Value cannot be empty.",
//                        Data = null
//                    });
//                }

//                _logger.LogInformation("POST request received with Key: {Key}, Value: {Value}", requestModel.Key, requestModel.Value);



//                return Ok(new ResponseModel<string>
//                {
//                    Success = true,
//                    Message = "Received successfully",
//                    Data = $"Key: {requestModel.Key}, Value: {requestModel.Value}"
//                });
//            }

//            catch (Exception ex)
//            {
//                var errorResponse = ExceptionHandler.CreateErrorResponse(ex, _logger);
//                return StatusCode(500, errorResponse);


//            }
//        }

//        [HttpPost("Save")]
//        public IActionResult SaveGreet([FromBody] GreetingEntity greeting)
//        {
//            try
//            {
//                if (string.IsNullOrWhiteSpace(greeting.Message))
//                {
//                    _logger.LogWarning("Empty greeting message received.");
//                    return BadRequest("Message cannot be empty.");
//                }
//                greeting.CreatedAt = DateTime.Now;

//                _greetingService.SaveGreeting(greeting.Message);
//                _logger.LogInformation("Greeting saved via API: {Message}", greeting.Message);

//                return Ok(new { Message = "Greeting saved successfully!" });
//            }
//            catch (Exception ex)
//            {
//                var errorResponse = ExceptionHandler.CreateErrorResponse(ex, _logger);
//                return StatusCode(500, errorResponse);


//            }
//        }

//        [HttpGet("getGreetingById/{id}")]
//        public IActionResult GetGreetingById(int id)
//        {
//            try
//            {

//                _logger.LogInformation($"Fetching greeting with ID: {id}");

//                var greeting = _greetingService.GetGreetingById(id);

//                if (greeting == null)
//                {
//                    return NotFound(new ResponseModel<string>
//                    {
//                        Success = false,
//                        Message = "Greeting not found",
//                        Data = null
//                    });
//                }

//                return Ok(new ResponseModel<GreetingEntity>
//                {
//                    Success = true,
//                    Message = "Greeting fetched successfully",
//                    Data = greeting
//                });
//            }
//            catch (Exception ex)
//            {
//                var errorResponse = ExceptionHandler.CreateErrorResponse(ex, _logger);
//                return StatusCode(500, errorResponse);


//            }
//        }
//        [HttpGet("getAll")]
//        public IActionResult GetAllGreetings()
//        {

//            try
//            {
//                _logger.LogInformation("Fetching all greetings from the repository.");
//                var greeting = _greetingService.GetAllGreeting();

//                if (greeting == null)
//                {
//                    return NotFound(new ResponseModel<string>
//                    {
//                        Success = false,
//                        Message = "Greeting not found",
//                        Data = null
//                    });
//                }
//                return Ok(new ResponseModel<List<GreetingEntity>> // for single object  (ResponseModel<GreetingEntity>) for Multiple List (ResponseModel<List<GreetingEntity>>)
//                {
//                    Success = true,
//                    Message = "Greeting fetched successfully",
//                    Data = greeting
//                });

//            }
//            catch (Exception ex)
//            {
//                var errorResponse = ExceptionHandler.CreateErrorResponse(ex, _logger);
//                return StatusCode(500, errorResponse);


//            }
//        }

//        [HttpPut("Update/{id}")]
//        public IActionResult updateGreeting(int id, [FromBody] EditGreeting editGreeting)
//        {

//            try {
//                _logger.LogInformation($"PUT request received for id:{id}");
//                bool isUpdate = _greetingService.EditGreeting(id, editGreeting);
//                if (!isUpdate)
//                {
//                    return NotFound(new ResponseModel<string>
//                    {
//                        Success = false,
//                        Message = "Id not Found",
//                        Data = null

//                    });

//                }

//                return Ok(new ResponseModel<string>
//                {

//                    Success = true,
//                    Message = "Greeting Updated Successfully",
//                    Data = editGreeting.NewMessage
//                });

//            }
//            catch (Exception ex)
//            {
//                var errorResponse = ExceptionHandler.CreateErrorResponse(ex, _logger);
//                return StatusCode(500, errorResponse);


//            }
//        }


//        [HttpDelete("delete/{id}")]

//        public IActionResult DeleteGreeting(int id)
//        {

//            try
//            {
//                _logger.LogInformation($"DELETE request received for id:{id}");
//                bool isDelete = _greetingService.deleteGreeting(id);

//                if (!isDelete)
//                {
//                    return NotFound(new ResponseModel<string>
//                    {

//                        Success = false,
//                        Message = "ID not Found",
//                        Data = null
//                    });
//                }

//                return Ok(new ResponseModel<string>
//                {
//                    Success = true,
//                    Message = "Successfully Deleted",
//                    Data = $"Deleted id :{id}"

//                });

//            }
//            catch (Exception ex)
//            {
//                var errorResponse = ExceptionHandler.CreateErrorResponse(ex, _logger);
//                return StatusCode(500, errorResponse);


//            }
//        }
//        [HttpPut]
//        public IActionResult Put(RequestModel requestModel)
//        {
//            try
//            {
//                _logger.LogInformation("PUT request received with Key: {Key}, Value: {Value}", requestModel.Key, requestModel.Value);

//                ResponseModel<string> responseModel = new ResponseModel<string>
//                {
//                    Success = true,
//                    Message = "Updated successfully",
//                    Data = $"Key: {requestModel.Key}, Value: {requestModel.Value}"
//                };

//                return Ok(responseModel);
//            }

//            catch (Exception ex)
//            {
//                var errorResponse = ExceptionHandler.CreateErrorResponse(ex, _logger);
//                return StatusCode(500, errorResponse);


//            }
//        }

//        [HttpPatch]
//        public IActionResult Patch(RequestModel requestModel)
//        {

//            try
//            {
//                _logger.LogInformation("PATCH request received with Key: {Key}, Value: {Value}", requestModel.Key, requestModel.Value);
//                ResponseModel<string> responseModel = new ResponseModel<string>
//                {
//                    Success = true,
//                    Message = "Partially updated successfully",
//                    Data = $"Key: {requestModel.Key}, Value: {requestModel.Value}"
//                };

//                return Ok(responseModel);
//            }
//            catch (Exception ex)
//            {
//                var errorResponse = ExceptionHandler.CreateErrorResponse(ex, _logger);
//                return StatusCode(500, errorResponse);


//            }
//        }
//        [HttpDelete]
//        public IActionResult Delete(RequestModel requestModel)
//        {
//            try
//            {
//                _logger.LogInformation("DELETE request received for Key: {Key}", requestModel.Key);

//                ResponseModel<string> responseModel = new ResponseModel<string>
//                {
//                    Success = true,
//                    Message = "Deleted successfully",
//                    Data = $"Key: {requestModel.Key}, Value: {requestModel.Value}"
//                };

//                return Ok(responseModel);
//            }
//            catch (Exception ex)
//            {
//                var errorResponse = ExceptionHandler.CreateErrorResponse(ex, _logger);
//                return StatusCode(500, errorResponse);


//            }
//        }

//        }
//}
using System.IdentityModel.Tokens.Jwt;
using BussinessLayer;
using BussinessLayer.Interface;
using Microsoft.AspNetCore.Mvc;
using Middleware;
using ModelLayer.Model;
using RepositoryLayer.Context;
using RepositoryLayer.Entity;
using RepositoryLayer.Helper;

namespace HelloGreetingApplication.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class HelloGreetingController : ControllerBase
    {
        private readonly ILogger<HelloGreetingController> _logger;
        private readonly IGreetingService _greetingService;
        private readonly UserContext _context;
        private readonly JwtHelper _jwtHelper;

        public HelloGreetingController(ILogger<HelloGreetingController> logger, IGreetingService greetingService, UserContext context, JwtHelper jwtHelper)
        {
            _logger = logger;
            _greetingService = greetingService;
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _jwtHelper = jwtHelper;
        }

        [HttpGet("getGreet")]
        public IActionResult GetGreetings()
        {
            _logger.LogInformation("Fetching greetings from database.");
            try
            {
                var greetings = _context.Set<GreetingEntity>().ToList();
                _logger.LogInformation("Fetched {Count} greetings successfully.", greetings.Count);
                return Ok(new { Success = true, Data = greetings });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while fetching greetings.");
                return StatusCode(500, "Internal Server Error.");
            }
        }

        [HttpPost("getToken")]
        public IActionResult GetToken()
        {
            _logger.LogInformation("Generating JWT token.");
            try
            {
                string token = _jwtHelper.GenerateToken(1, "Varsha", "varsha@example.com", "Pass@123");
                _logger.LogInformation("JWT token generated successfully.");
                return Ok(new { Token = token });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while generating JWT token.");
                return StatusCode(500, "Internal Server Error.");
            }
        }

        [HttpGet("validateToken")]
        public IActionResult ValidateToken([FromBody] TokenRequestcs tokenRequest)
        {
            _logger.LogInformation("Validating JWT token.");
            try
            {
                var claims = _jwtHelper.ValidateToken(tokenRequest.Token);
                if (claims == null)
                {
                    _logger.LogWarning("Invalid token received.");
                    return Unauthorized("Invalid token");
                }

                _logger.LogInformation("Token validated successfully.");
                return Ok(new { Message = "Token is valid", Claims = claims.Claims.Select(c => new { c.Type, c.Value }) });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while validating token.");
                return StatusCode(500, "Internal Server Error.");
            }
        }

        [HttpGet("getGreetingById/{id}")]
        public IActionResult GetGreetingById(int id)
        {
            _logger.LogInformation("Fetching greeting with ID: {Id}", id);
            try
            {
                var greeting = _greetingService.GetGreetingById(id);
                if (greeting == null)
                {
                    _logger.LogWarning("Greeting with ID: {Id} not found.", id);
                    return NotFound(new { Success = false, Message = "Greeting not found." });
                }
                return Ok(new { Success = true, Message = "Greeting fetched successfully.", Data = greeting });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while fetching greeting.");
                return StatusCode(500, "Internal Server Error.");
            }
        }

        [HttpGet("getAll")]
        public IActionResult GetAllGreetings()
        {
            _logger.LogInformation("Fetching all greetings.");
            try
            {
                var greetings = _greetingService.GetAllGreeting();
                return Ok(new { Success = true, Message = "Greetings fetched successfully.", Data = greetings });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while fetching all greetings.");
                return StatusCode(500, "Internal Server Error.");
            }
        }

        [HttpPost("Save")]
        public IActionResult SaveGreet([FromBody] GreetingEntity greeting)
        {
            _logger.LogInformation("Received request to save greeting.");
            try
            {
                if (string.IsNullOrWhiteSpace(greeting.Message))
                {
                    _logger.LogWarning("Empty greeting message received.");
                    return BadRequest("Message cannot be empty.");
                }

                greeting.CreatedAt = DateTime.Now;
                _greetingService.SaveGreeting(greeting.Message);
                _logger.LogInformation("Greeting saved successfully: {Message}", greeting.Message);

                return Ok(new { Message = "Greeting saved successfully!" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while saving greeting.");
                return StatusCode(500, "Internal Server Error.");
            }
        }

        [HttpPut("Update/{id}")]
        public IActionResult UpdateGreeting(int id, [FromBody] EditGreeting editGreeting)
        {
            _logger.LogInformation("Updating greeting with ID: {Id}", id);
            try
            {
                bool isUpdated = _greetingService.EditGreeting(id, editGreeting);
                if (!isUpdated)
                {
                    _logger.LogWarning("Greeting with ID: {Id} not found.", id);
                    return NotFound(new { Success = false, Message = "Greeting not found." });
                }

                _logger.LogInformation("Greeting updated successfully.");
                return Ok(new { Success = true, Message = "Greeting updated successfully." });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while updating greeting.");
                return StatusCode(500, "Internal Server Error.");
            }
        }

        [HttpDelete("delete/{id}")]
        public IActionResult DeleteGreeting(int id)
        {
            _logger.LogInformation("Received request to delete greeting with ID: {Id}", id);
            try
            {
                bool isDeleted = _greetingService.deleteGreeting(id);
                if (!isDeleted)
                {
                    _logger.LogWarning("Greeting with ID: {Id} not found.", id);
                    return NotFound(new { Success = false, Message = "Greeting not found." });
                }

                _logger.LogInformation("Greeting with ID: {Id} deleted successfully.", id);
                return Ok(new { Success = true, Message = "Greeting deleted successfully." });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while deleting greeting.");
                return StatusCode(500, "Internal Server Error.");
            }
        }

        [HttpPost]
        public IActionResult Post(RequestModel requestModel)
        {
            _logger.LogInformation("POST request received with Key: {Key}, Value: {Value}", requestModel.Key, requestModel.Value);
            try
            {
                return Ok(new { Success = true, Message = "Received successfully.", Data = requestModel });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while handling POST request.");
                return StatusCode(500, "Internal Server Error.");
            }
        }

        [HttpPut]
        public IActionResult Put(RequestModel requestModel)
        {
            _logger.LogInformation("PUT request received with Key: {Key}, Value: {Value}", requestModel.Key, requestModel.Value);
            try
            {
                return Ok(new { Success = true, Message = "Updated successfully.", Data = requestModel });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while handling PUT request.");
                return StatusCode(500, "Internal Server Error.");
            }
        }

        [HttpPatch]
        public IActionResult Patch(RequestModel requestModel)
        {
            _logger.LogInformation("PATCH request received with Key: {Key}, Value: {Value}", requestModel.Key, requestModel.Value);
            try
            {
                return Ok(new { Success = true, Message = "Partially updated successfully.", Data = requestModel });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while handling PATCH request.");
                return StatusCode(500, "Internal Server Error.");
            }
        }

        [HttpDelete]
        public IActionResult Delete(RequestModel requestModel)
        {
            _logger.LogInformation("DELETE request received for Key: {Key}", requestModel.Key);
            try
            {
                return Ok(new { Success = true, Message = "Deleted successfully.", Data = requestModel });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while handling DELETE request.");
                return StatusCode(500, "Internal Server Error.");
            }
        }
    }
}
