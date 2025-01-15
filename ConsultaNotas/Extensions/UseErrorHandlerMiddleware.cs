using ConsultaNotas.Middlewares;

namespace ConsultaNotas.Extensions
{
    public static class UseErrorHandlerMiddleware
    {
        public static void UseErrorHandlingMiddleware(this IApplicationBuilder app)
        {
            app.UseMiddleware<ErrorHandlerMiddleware>();
        }
    }
}
