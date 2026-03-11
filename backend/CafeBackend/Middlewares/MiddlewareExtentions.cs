

namespace CafeBackend.Middlewares
{
    public static class MiddlewareExtension
    {
        public static IApplicationBuilder UseCustomCors(this ApplicationBuilder builder)
        {
            return builder.UseMiddleware<CorsMiddleware>();
        }
    }
}