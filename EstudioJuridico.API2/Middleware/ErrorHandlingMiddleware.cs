using System.Net;
using System.Text.Json;

namespace EstudioJuridico.API2.Middleware
{
    public class ErrorHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ErrorHandlingMiddleware> _logger;

        public ErrorHandlingMiddleware(RequestDelegate next, ILogger<ErrorHandlingMiddleware> logger)
        {
            _next   = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (ArgumentException ex)
            {
                // Errores de validación (ValidarRequerido, ValidarPositivo, etc.)
                _logger.LogWarning(ex, "Error de validación");
                await EscribirRespuesta(context, HttpStatusCode.BadRequest, ex.Message);
            }
            catch (UnauthorizedAccessException ex)
            {
                // Errores de permisos
                _logger.LogWarning(ex, "Error de permisos");
                await EscribirRespuesta(context, HttpStatusCode.Forbidden, "No tenés permisos para esta acción.");
            }
            catch (KeyNotFoundException ex)
            {
                // Recursos no encontrados
                _logger.LogWarning(ex, "Recurso no encontrado");
                await EscribirRespuesta(context, HttpStatusCode.NotFound, ex.Message);
            }
            catch (Exception ex)
            {
                // Cualquier otro error inesperado
                _logger.LogError(ex, "Error no controlado en {Path}", context.Request.Path);
                await EscribirRespuesta(context, HttpStatusCode.InternalServerError,
                    "Ocurrió un error interno. Contactá al administrador.");
            }
        }

        private static async Task EscribirRespuesta(HttpContext context, HttpStatusCode statusCode, string mensaje)
        {
            context.Response.StatusCode  = (int)statusCode;
            context.Response.ContentType = "application/json";

            var respuesta = JsonSerializer.Serialize(new
            {
                success = false,
                mensaje
            });

            await context.Response.WriteAsync(respuesta);
        }
    }
}