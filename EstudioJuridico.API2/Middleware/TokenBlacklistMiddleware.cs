using EstudioJuridico.API2.Services;

namespace EstudioJuridico.API2.Middleware
{
    public class TokenBlacklistMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly TokenBlacklistService _blacklist;

        public TokenBlacklistMiddleware(RequestDelegate next, TokenBlacklistService blacklist)
        {
            _next     = next;
            _blacklist = blacklist;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var token = context.Request.Headers["Authorization"]
                .ToString().Replace("Bearer ", "");

            if (!string.IsNullOrEmpty(token) && _blacklist.EsInvalido(token))
            {
                context.Response.StatusCode = 401;
                await context.Response.WriteAsJsonAsync(new
                {
                    success = false,
                    mensaje = "Token inválido. Por favor iniciá sesión nuevamente."
                });
                return;
            }

            await _next(context);
        }
    }
}