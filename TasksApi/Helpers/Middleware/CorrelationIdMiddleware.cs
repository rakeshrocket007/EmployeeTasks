using Microsoft.Extensions.Primitives;
using Serilog.Context;

namespace TasksApi.Helpers.Middleware
{
    public class CorrelationIdMiddleware
    {
        private readonly RequestDelegate _Next;

        public CorrelationIdMiddleware(RequestDelegate next)
        {
            this._Next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            context.Request.Headers.TryGetValue("Correlation-Id-Header", out StringValues correlationIds);
            Guid requestCorrelationId;                
            Guid correlationId;
            if(Guid.TryParse(correlationIds.FirstOrDefault(), out requestCorrelationId))
            {
                correlationId = Guid.NewGuid();
                context.Request.Headers.Add("Correlation-Id-Header", correlationId.ToString());
            } 
            else
            {
                correlationId = requestCorrelationId;
            }

            using (LogContext.PushProperty("CorrelationId", correlationId))
            {
                await _Next(context);
            }
        }
    }
}
