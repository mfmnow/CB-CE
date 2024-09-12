using Mfm.Bc.CurrencyExchanger.Domain.Models.Exceptions;
using System.Net;
using System.Text.Json;

namespace Mfm.Bc.CurrencyExchanger.WebApi.App_Code.Middlewares
{
    /// <summary>
    /// ErrorHandlerMiddleware class
    /// </summary>
    public class ErrorHandlerMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ErrorHandlerMiddleware> _logger;

        /// <summary>
        /// ErrorHandlerMiddleware
        /// </summary>
        /// <param name="next"></param>
        /// <param name="logger"></param>
        public ErrorHandlerMiddleware(RequestDelegate next, ILogger<ErrorHandlerMiddleware> logger)
        {
            _logger = logger;
            _next = next;
        }

        /// <summary>
        /// Invoke
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception error)
            {
                var response = context.Response;
                response.ContentType = "application/json";
                var result = string.Empty;

                switch (error)
                {
                    case BusinessValidationException businessValidationException:
                        // custom application error
                        response.StatusCode = (int)HttpStatusCode.BadRequest;
                        result = JsonSerializer.Serialize(new { message = businessValidationException.Message, traceId = context.TraceIdentifier });
                        _logger.LogWarning(message: businessValidationException.Message , exception: error, args: GetContextArgs(context));
                        break;
                    case NotFoundException notFoundException:
                        // custom application error
                        response.StatusCode = (int)HttpStatusCode.NotFound;
                        _logger.LogInformation(message: notFoundException.Message, exception: error, args: GetContextArgs(context));
                        break;
                    case KeyNotFoundException keyNotFoundException:
                        // not found error
                        response.StatusCode = (int)HttpStatusCode.NotFound;
                        _logger.LogWarning(message: keyNotFoundException.Message, exception: error, args: new { traceId = context.TraceIdentifier });
                        break;
                    default:
                        // unhandled error
                        response.StatusCode = (int)HttpStatusCode.InternalServerError;
                        result = JsonSerializer.Serialize(new { traceId = context.TraceIdentifier });
                        _logger.LogError(message: error.Message, exception: error, args: GetContextArgs(context));
                        _logger.LogInformation(message: error.Message, exception: error, args: GetContextArgs(context));
                        break;
                }
                await response.WriteAsync(result);
            }
        }

        /// <summary>
        /// Generates Context Args
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private object GetContextArgs(HttpContext context)
        {
            return new
            {
                traceId = context?.TraceIdentifier,
                url = context?.Request?.Path+ context?.Request?.QueryString,
                method = context?.Request?.Method
            };
        }
    }
}
