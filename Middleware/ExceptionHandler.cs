using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
//using NLog;
using Microsoft.Extensions.Logging;

namespace Middleware
{
  public static class ExceptionHandler
    {
       
        public static string HandleException(Exception ex, ILogger logger, out object errorResponse)
        {
            logger.LogError(ex, "An error occurred in the application");

            errorResponse = new
            {
                Success = false,
                Message = "An error occurred",
                Error = ex.Message
            };

            return JsonConvert.SerializeObject(errorResponse);
        }

        // Optional: Create an error response without serialization
        public static object CreateErrorResponse(Exception ex, ILogger logger)
        {
            logger.LogError(ex, "An error occurred in the application");

            return new
            {
                Success = false,
                Message = "An error occurred",
                Error = ex.Message
            };
        }
    }
    }



