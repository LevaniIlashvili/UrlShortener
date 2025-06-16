﻿using FluentValidation;
using System.Net;
using System.Text.Json;
using UrlShortener.Application.Exceptions;

namespace UrlShortener.Api.Middlewares
{
    public class ExceptionHandlerMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionHandlerMiddleware> _logger;

        public ExceptionHandlerMiddleware(RequestDelegate next, ILogger<ExceptionHandlerMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                await ConvertException(context, ex);
            }
        }

        private Task ConvertException(HttpContext context, Exception exception)
        {
            HttpStatusCode httpStatusCode = HttpStatusCode.InternalServerError;

            context.Response.ContentType = "application/json";

            var result = string.Empty;

            switch (exception)
            {
                case ValidationException validationException:
                    httpStatusCode = HttpStatusCode.BadRequest;
                    result = JsonSerializer.Serialize(new
                    {
                        errors = validationException.Errors
                            .Select(e => new { field = e.PropertyName, message = e.ErrorMessage })
                    });
                    break;

                case NotFoundException:
                    httpStatusCode = HttpStatusCode.NotFound;
                    break;

                case ConflictException:
                    httpStatusCode = HttpStatusCode.Conflict;
                    break;

                default:
                    _logger.LogError(exception, "An unexpected error occurred.");
                    result = JsonSerializer.Serialize(new { error = "An unexpected error occurred. Please try again later." });
                    break;
            }

            context.Response.StatusCode = (int)httpStatusCode;

            if (result == string.Empty)
            {
                result = JsonSerializer.Serialize(new { error = exception.Message });
            }

            return context.Response.WriteAsync(result);
        }
    }
}
