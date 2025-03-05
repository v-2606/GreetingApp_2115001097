using BussinessLayer;
using BussinessLayer.Interface;
using BussinessLayer.Service;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ModelLayer.Model;
using RepositoryLayer.Context;
using RepositoryLayer.Entity;

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



        /// <summary>
        /// Constructor to initialize logger.
        /// </summary>
        public HelloGreetingController(ILogger<HelloGreetingController> logger, IGreetingService greetingService, UserContext context)
        {
        _logger = logger;
            _greetingService = greetingService;
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        /// <summary>
        /// Get Method to get the greeting message
        /// </summary>
        /// <returns> Hello World!</returns>
        /// 

        [HttpGet("getGreet")]
        public IActionResult GetGreetings()
        {
            var greetings = _context.Set<GreetingEntity>().ToList(); 

            return Ok(new { Success = true, Data = greetings });
        }


        [HttpGet]
        public IActionResult Get(string? firstName = "", string? lastName = "")
        {

            _logger.LogInformation("GET request received.");
            string message = _greetingService.GetGreetingMessage(firstName, lastName);
            ResponseModel<string> responseModel = new ResponseModel<string>();
            responseModel.Success = true;
            responseModel.Message = " Hello to Greeting App API EndPoint";
            responseModel.Data = message;
            return Ok(responseModel);
        }


        [HttpPost]
        public IActionResult Post(RequestModel requestModel)
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

            var responseModel = new ResponseModel<string>
            {
                Success = true,
                Message = "Received successfully",
                Data = $"Key: {requestModel.Key}, Value: {requestModel.Value}"
            };

            return Ok(responseModel);
        }

        [HttpPost("Save")]
        public IActionResult SaveGreet([FromBody] GreetingEntity greeting)
        {
            if (string.IsNullOrWhiteSpace(greeting.Message))
            {
                _logger.LogWarning("Empty greeting message received.");
                return BadRequest("Message cannot be empty.");
            }
             greeting.CreatedAt = DateTime.Now;

            _greetingService.SaveGreeting(greeting.Message);
            _logger.LogInformation("Greeting saved via API: {Message}",greeting.Message);

            return Ok(new { Message = "Greeting saved successfully!" });
        }


        [HttpGet("getGreetingById/{id}")]
        public IActionResult GetGreetingById(int id)
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
       
        [HttpGet("getAll")]
        public IActionResult GetAllGreetings()
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

        [HttpPut("Update/{id}")]
        public IActionResult updateGreeting(int id, [FromBody] EditGreeting editGreeting )
        {
            _logger.LogInformation($"PUT request received for id:{id}");
            bool isUpdate=_greetingService.EditGreeting(id, editGreeting);
            if (!isUpdate)
            { 
              return NotFound( new ResponseModel<string>
              {
                  Success=false,
                  Message="Id not Found",
                  Data=null

              });

            }

            return Ok(new ResponseModel<string>
            {

                Success = true,
                Message = "Greeting Updated Successfully",
                Data = editGreeting.NewMessage
            });

        }


        [HttpDelete("delete/{id}")]

        public IActionResult DeleteGreeting(int id) {
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
                Success=true,
                Message="Successfully Deleted",
                Data=$"Deleted id :{id}"
            
            });

        }
            [HttpPut]
        public IActionResult Put(RequestModel requestModel)
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

        [HttpPatch]
        public IActionResult Patch(RequestModel requestModel)
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

        [HttpDelete]
        public IActionResult Delete(RequestModel requestModel)
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
      
    }
}
