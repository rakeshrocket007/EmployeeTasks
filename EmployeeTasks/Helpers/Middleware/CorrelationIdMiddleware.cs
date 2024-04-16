namespace EmployeeTasks.Helpers.Middleware
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
            var header = context.Request.Headers["CorrelationId"];
            string sessionId;

            if (header.Count > 0)
            {
                sessionId = header[0];
            }
            else
            {
                sessionId = Guid.NewGuid().ToString();
            }

            context.Items["CorrelationId"] = sessionId;
            await this._Next(context);
        }
    }
}
