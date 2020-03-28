using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Domain.Exceptions;

namespace WebApi.Middlewares
{
    public class ErrorHandlerMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger _logger;

        public ErrorHandlerMiddleware(RequestDelegate next, ILogger<ErrorHandlerMiddleware> logger)
        {
            this._next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (ModelException ex)
            {
                var request = context.Request;
                _logger.LogError($"Error: {ex.ToLogString()}, Request: {request.Path}");
                await HandleExceptionAsync(context, ex);
            }

        }

        private static Task HandleExceptionAsync(HttpContext context, ModelException ex)
        {           
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = ex.StatusCode;
            return context.Response.WriteAsync(ex.ToString());
        }
    }
}
