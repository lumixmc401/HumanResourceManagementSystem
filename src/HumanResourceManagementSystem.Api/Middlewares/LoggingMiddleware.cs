using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Diagnostics;

namespace HumanResourceManagementSystem.Api.Middlewares
{
    public class LoggingMiddleware(RequestDelegate next, ILogger<LoggingMiddleware> logger)
    {
        private readonly RequestDelegate _next = next ?? throw new ArgumentNullException(nameof(next));
        private readonly ILogger<LoggingMiddleware> _logger = logger ?? throw new ArgumentNullException(nameof(logger));

        public async Task InvokeAsync(HttpContext context)
        {
            var request = context.Request;
            var user = context.User?.Identity?.IsAuthenticated == true ? context.User.Identity.Name : "Anonymous";
            var ipAddress = context.Connection.RemoteIpAddress?.ToString();
            var traceId = context.TraceIdentifier;
            string requestData = string.Empty;

            try
            {
                if (request.Method == HttpMethods.Get)
                {
                    requestData = JsonConvert.SerializeObject(request.Query);
                }
                else if (request.HasFormContentType && request.Form.Files.Count > 0)
                {
                    var files = request.Form.Files;
                    var fileInfoList = new List<object>();
                    foreach (var file in files)
                    {
                        fileInfoList.Add(new
                        {
                            file.FileName,
                            file.ContentType,
                            file.Length
                        });
                    }
                    requestData = JsonConvert.SerializeObject(fileInfoList);
                }
                else
                {
                    request.EnableBuffering();
                    using var reader = new StreamReader(request.Body, leaveOpen: true);
                    requestData = await reader.ReadToEndAsync();
                    request.Body.Position = 0;
                }

                _logger.LogInformation("Handling request {RequestPath} from user {User} with IP {IPAddress} and TraceID {TraceID}. Request data: {RequestData}",
                    request.Path, user, ipAddress, traceId, requestData);

                var timer = new Stopwatch();
                timer.Start();

                await _next(context);

                timer.Stop();
                var timeTaken = timer.Elapsed;

                if (timeTaken.TotalSeconds > 3)
                {
                    _logger.LogWarning("The request {RequestPath} took {TimeTaken} seconds. TraceID: {TraceID}",
                        request.Path, timeTaken.TotalSeconds, traceId);
                }

                _logger.LogInformation("Finished handling request {RequestPath} from user {User} with IP {IPAddress} and TraceID {TraceID}",
                    request.Path, user, ipAddress, traceId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while processing the request. TraceID: {TraceID}", traceId);
                throw;
            }
        }
    }
}
