//using System.IdentityModel.Tokens.Jwt;
using BussinessLayer;
using BussinessLayer.Interface;
using BussinessLayer.Service;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Middleware;
using ModelLayer.Model;
using RepositoryLayer.Context;
using RepositoryLayer.Entity;
using RepositoryLayer.Helper;

namespace HelloGreetingApplication.Controllers
{

    /// <summary>
    /// class providing API for HelloGreeting
    /// </summary>


    [ApiController]
    [Route("[controller]")]
    public class HelloGreetingController : ControllerBase

    {
        private readonly ILogger<HelloGreetingController> _logger;
        private readonly IGreetingService _greetingService;
        private readonly UserContext _context;
      //  private readonly JwtHelper _jwtHelper;


        /// <summary>
        /// Constructor to initialize logger.
        /// </summary>
        public HelloGreetingController(ILogger<HelloGreetingController> logger, IGreetingService greetingService, UserContext context) { 
            _logger = logger;
            _greetingService = greetingService;
            _context = context ?? throw new ArgumentNullException(nameof(context));
           // _jwtHelper = jwtHelper;
        }

        /// <summary>
        /// Get Method to get the greeting message
        /// </summary>
        /// <returns> Hello World!</returns>
        /// 

        [HttpGet("getGreet")]
        public IActionResult GetGreetings()
        {
            _logger.LogInformation("Fetching greetings from database.");
            try
            {
                var greetings = _context.Set<GreetingEntity>().ToList();

                return Ok(new { Success = true, Data = greetings });
            }

            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while fetching greetings.");
                var errorResponse = ExceptionHandler.CreateErrorResponse(ex, _logger);
                return StatusCode(500, errorResponse);


            }
        }
      

        [HttpGet]
        public IActionResult Get(string? firstName = "", string? lastName = "")
        {
            try
            {
                _logger.LogInformation("GET request received.");
                string message = _greetingService.GetGreetingMessage(firstName, lastName);
                ResponseModel<string> responseModel = new ResponseModel<string>();
                responseModel.Success = true;
                responseModel.Message = " Hello to Greeting App API EndPoint";
                responseModel.Data = message;
                return Ok(responseModel);
            }

            catch (Exception ex)
            {
                var errorResponse = ExceptionHandler.CreateErrorResponse(ex, _logger);
                return StatusCode(500, errorResponse);


            }
        }

        [HttpPost]
        public IActionResult Post(RequestModel requestModel)
        {

            try
            {
                if (string.IsNullOrEmpty(requestModel?.Key) || string.IsNullOrEmpty(requestModel?.Value))
                {
                    return BadRequest(new ResponseModel<string>
                    {
                        Success = false,
                        Message = "Invalid request. Key and Value cannot be empty.",
                        Data = null
                    });
                }

                _logger.LogInformation("POST request received with Key: {Key}, Value: {Value}", requestModel.Key, requestModel.Value);



                return Ok(new ResponseModel<string>
                {
                    Success = true,
                    Message = "Received successfully",
                    Data = $"Key: {requestModel.Key}, Value: {requestModel.Value}"
                });
            }

            catch (Exception ex)
            {
                var errorResponse = ExceptionHandler.CreateErrorResponse(ex, _logger);
                return StatusCode(500, errorResponse);


            }
        }

        [HttpPost("Save")]
        public IActionResult SaveGreet([FromBody] GreetingEntity greeting)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(greeting.Message))
                {
                    _logger.LogWarning("Empty greeting message received.");
                    return BadRequest("Message cannot be empty.");
                }
                greeting.CreatedAt = DateTime.Now;

                _greetingService.SaveGreeting(greeting.Message);
                _logger.LogInformation("Greeting saved via API: {Message}", greeting.Message);

                return Ok(new { Message = "Greeting saved successfully!" });
            }
            catch (Exception ex)
            {
                var errorResponse = ExceptionHandler.CreateErrorResponse(ex, _logger);
                return StatusCode(500, errorResponse);


            }
        }

        [HttpGet("getGreetingById/{id}")]
        public IActionResult GetGreetingById(int id)
        {
            try
            {

                _logger.LogInformation($"Fetching greeting with ID: {id}");

                var greeting = _greetingService.GetGreetingById(id);

                if (greeting == null)
                {
                    return NotFound(new ResponseModel<string>
                    {
                        Success = false,
                        Message = "Greeting not found",
                        Data = null
                    });
                }

                return Ok(new ResponseModel<GreetingEntity>
                {
                    Success = true,
                    Message = "Greeting fetched successfully",
                    Data = greeting
                });
            }
            catch (Exception ex)
            {
                var errorResponse = ExceptionHandler.CreateErrorResponse(ex, _logger);
                return StatusCode(500, errorResponse);


            }
        }
        [HttpGet("getAll")]
        public IActionResult GetAllGreetings()
        {

            try
            {
                _logger.LogInformation("Fetching all greetings from the repository.");
                var greeting = _greetingService.GetAllGreeting();

                if (greeting == null)
                {
                    return NotFound(new ResponseModel<string>
                    {
                        Success = false,
                        Message = "Greeting not found",
                        Data = null
                    });
                }
                return Ok(new ResponseModel<List<GreetingEntity>> // for single object  (ResponseModel<GreetingEntity>) for Multiple List (ResponseModel<List<GreetingEntity>>)
                {
                    Success = true,
                    Message = "Greeting fetched successfully",
                    Data = greeting
                });

            }
            catch (Exception ex)
            {
                var errorResponse = ExceptionHandler.CreateErrorResponse(ex, _logger);
                return StatusCode(500, errorResponse);


            }
        }

        [HttpPut("Update/{id}")]
        public IActionResult updateGreeting(int id, [FromBody] EditGreeting editGreeting)
        {

            try
            {
                _logger.LogInformation($"PUT request received for id:{id}");
                bool isUpdate = _greetingService.EditGreeting(id, editGreeting);
                if (!isUpdate)
                {
                    return NotFound(new ResponseModel<string>
                    {
                        Success = false,
                        Message = "Id not Found",
                        Data = null

                    });

                }

                return Ok(new ResponseModel<string>
                {

                    Success = true,
                    Message = "Greeting Updated Successfully",
                    Data = editGreeting.NewMessage
                });

            }
            catch (Exception ex)
            {
                var errorResponse = ExceptionHandler.CreateErrorResponse(ex, _logger);
                return StatusCode(500, errorResponse);


            }
        }


        [HttpDelete("delete/{id}")]

        public IActionResult DeleteGreeting(int id)
        {

            try
            {
                _logger.LogInformation($"DELETE request received for id:{id}");
                bool isDelete = _greetingService.deleteGreeting(id);

                if (!isDelete)
                {
                    return NotFound(new ResponseModel<string>
                    {

                        Success = false,
                        Message = "ID not Found",
                        Data = null
                    });
                }

                return Ok(new ResponseModel<string>
                {
                    Success = true,
                    Message = "Successfully Deleted",
                    Data = $"Deleted id :{id}"

                });

            }
            catch (Exception ex)
            {
                var errorResponse = ExceptionHandler.CreateErrorResponse(ex, _logger);
                return StatusCode(500, errorResponse);


            }
        }
        [HttpPut]
        public IActionResult Put(RequestModel requestModel)
        {
            try
            {
                _logger.LogInformation("PUT request received with Key: {Key}, Value: {Value}", requestModel.Key, requestModel.Value);

                ResponseModel<string> responseModel = new ResponseModel<string>
                {
                    Success = true,
                    Message = "Updated successfully",
                    Data = $"Key: {requestModel.Key}, Value: {requestModel.Value}"
                };

                return Ok(responseModel);
            }

            catch (Exception ex)
            {
                var errorResponse = ExceptionHandler.CreateErrorResponse(ex, _logger);
                return StatusCode(500, errorResponse);


            }
        }

        [HttpPatch]
        public IActionResult Patch(RequestModel requestModel)
        {

            try
            {
                _logger.LogInformation("PATCH request received with Key: {Key}, Value: {Value}", requestModel.Key, requestModel.Value);
                ResponseModel<string> responseModel = new ResponseModel<string>
                {
                    Success = true,
                    Message = "Partially updated successfully",
                    Data = $"Key: {requestModel.Key}, Value: {requestModel.Value}"
                };

                return Ok(responseModel);
            }
            catch (Exception ex)
            {
                var errorResponse = ExceptionHandler.CreateErrorResponse(ex, _logger);
                return StatusCode(500, errorResponse);


            }
        }
        [HttpDelete]
        public IActionResult Delete(RequestModel requestModel)
        {
            try
            {
                _logger.LogInformation("DELETE request received for Key: {Key}", requestModel.Key);

                ResponseModel<string> responseModel = new ResponseModel<string>
                {
                    Success = true,
                    Message = "Deleted successfully",
                    Data = $"Key: {requestModel.Key}, Value: {requestModel.Value}"
                };

                return Ok(responseModel);
            }
            catch (Exception ex)
            {
                var errorResponse = ExceptionHandler.CreateErrorResponse(ex, _logger);
                return StatusCode(500, errorResponse);


            }
        }

    }
}
