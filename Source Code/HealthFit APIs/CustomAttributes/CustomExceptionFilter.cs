using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc.Filters;

namespace HealthFit.CustomAttributes
{
    public class CustomExceptionFilter : IExceptionFilter
    {
        private readonly ILogger<CustomExceptionFilter> _logger;

        public CustomExceptionFilter(ILogger<CustomExceptionFilter> logger)
        {
            _logger = logger;
        }

        public void OnException(ExceptionContext context)
        {
            _logger.LogError(context.Exception, "An error occurred.");

            context.Result = new ContentResult
            {
                Content = "An error occurred. Please try again later.",
                StatusCode = 500
            };

            context.ExceptionHandled = true;
        }
    }
}
