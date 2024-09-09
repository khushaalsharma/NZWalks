using System.Net;

namespace NZWalks.API.Middlewares
{
    public class ExceptionHandlerMiddleware
    {
        private readonly ILogger<ExceptionHandlerMiddleware> logger;
        private readonly RequestDelegate next;
       
        public ExceptionHandlerMiddleware(ILogger<ExceptionHandlerMiddleware> logger, RequestDelegate next)
        { //request delegate returns a task that represents completion of request processing
            this.logger = logger;
            this.next = next;
        }

        public async Task InvokeAsync(HttpContext httpContext)
        {
            try
            {
                await next(httpContext); //if anything happens between the calls exception will be called
                
            }
            catch (Exception e)
            {
                var errorId = Guid.NewGuid();
                //logs this exception
                logger.LogError(e, $"{errorId} : {e.Message}");

                //return a custom error response

                httpContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                httpContext.Response.ContentType = "application/json";

                var errorModel = new
                {
                    Id = errorId,
                    errorMessage = "Something went wrong! We are looking to resolving this.",
                };

                await httpContext.Response.WriteAsJsonAsync(errorModel);
            }
        }
    }
}
