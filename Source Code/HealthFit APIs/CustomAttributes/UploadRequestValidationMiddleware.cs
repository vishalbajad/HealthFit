using Microsoft.AspNetCore.Http;
using System;
using System.Threading.Tasks;
namespace HealthFit_APIs.CustomAttributes
{
    public class UploadRequestValidationMiddleware
    {
        private readonly RequestDelegate _next;

        public UploadRequestValidationMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            // Perform request validation checks here
            // For example, check request headers, content type, file size, etc.

            // If the request is valid, proceed to the next middleware
            await _next(context);
        }
    }
}