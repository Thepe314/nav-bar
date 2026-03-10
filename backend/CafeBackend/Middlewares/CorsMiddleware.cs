namespace CafeBackend.Middlewares
{
    public class CorsMiddleware
    {
        //readonly
        private readonly RequestDelegate _next;

        public  CorsMiddleware(RequestDelegate next) => _next = next;

        public async Task InvokeAsync(HttpContext context)
        {
            context.Response.Headers.Append("Access-Control-Allow-Origin", "*");
            context.Response.Headers.Append("Access-Control-Allow-Methods", "GET,POST,PUT,DELETE,OPTIONS");
            context.Response.Headers.Append("Access-Control-Allow-Headers", "Content-Type,Authorization");

            if(context.Request.Method == "OPTIONS")
            {
                context.Response.StatusCode = 200;
                return;
            }
             await _next(context);
        }

       
    }
}