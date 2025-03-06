using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using Microsoft.Extensions.Logging;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;



namespace Middleware
{
    public class GlobalExceptionFilter: IExceptionFilter
    {
        private readonly ILogger<GlobalExceptionFilter> _logger;

        public GlobalExceptionFilter(ILogger<GlobalExceptionFilter> logger)
        {
            _logger = logger;
        }

        public void OnException(ExceptionContext context)
        {
            // Use the ExceptionHandler utility to create the error response
            var errorResponse = ExceptionHandler.CreateErrorResponse(context.Exception, _logger);

            context.Result = new ObjectResult(errorResponse)
            {
                StatusCode = 500 // Internal Server Error
            };

            context.ExceptionHandled = true; // Mark the exception as handled
        }

    }
}



