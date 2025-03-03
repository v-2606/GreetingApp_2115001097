using BussinessLayer.Interface;
using BussinessLayer.Service;
using Microsoft.AspNetCore.Mvc;
using ModelLayer.Model;

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

       

        /// <summary>
        /// Constructor to initialize logger.
        /// </summary>
        public HelloGreetingController(ILogger<HelloGreetingController> logger, IGreetingService greetingService)
        {
        _logger = logger;
            _greetingService = greetingService;
        }
    
        /// <summary>
        /// Get Method to get the greeting message
        /// </summary>
        /// <returns> Hello World!</returns>
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
