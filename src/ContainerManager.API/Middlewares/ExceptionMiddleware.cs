using ContainerManager.API.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Net;
using System.Threading.Tasks;

namespace ContainerManager.API.Middlewares
{
	public class ExceptionMiddleware
	{
		private readonly RequestDelegate _next;
		private readonly ILogger<ExceptionMiddleware> _logger;


		public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger)
		{
			_next = next;
			_logger = logger;
		}

		public async Task InvokeAsync(HttpContext httpContext)
		{
			try
			{
				await _next(httpContext);
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "Unhandled Error: {ErrorMessage}", ex.Message);
				await HandleExceptionAsync(httpContext);
			}
		}

		private static async Task HandleExceptionAsync(HttpContext context)
		{
			context.Response.ContentType = "application/json";
			context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

			await context.Response.WriteAsync(new ErrorResponse
			{
				StatusCode = context.Response.StatusCode,
				Message = "Internal Server Error. Admin can check Logs for details"
			}.ToString());
		}
	}
}
