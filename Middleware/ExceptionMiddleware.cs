using System.Net;
using System.Text.Json;
using TareasApi.Exceptions;

namespace TareasApi.Middleware;

/// <summary>
/// Intercepta cualquier excepción no controlada lanzada durante el pipeline
/// y la traduce a una respuesta JSON consistente con el código HTTP correcto.
/// </summary>
public class ExceptionMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionMiddleware> _logger;

    public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            await ManejarExcepcionAsync(context, ex);
        }
    }

    private async Task ManejarExcepcionAsync(HttpContext context, Exception ex)
    {
        var (statusCode, mensajes) = ex switch
        {
            AppValidationException validacion =>
                (HttpStatusCode.BadRequest, (IReadOnlyList<string>)validacion.Errores),

            BusinessRuleException regla =>
                (HttpStatusCode.BadRequest, new List<string> { regla.Message }),

            NotFoundException noEncontrada =>
                (HttpStatusCode.NotFound, new List<string> { noEncontrada.Message }),

            _ => (HttpStatusCode.InternalServerError, new List<string> { "Ocurrió un error interno en el servidor." })
        };

        if (statusCode == HttpStatusCode.InternalServerError)
        {
            _logger.LogError(ex, "Error no controlado procesando {Metodo} {Ruta}", context.Request.Method, context.Request.Path);
        }
        else
        {
            _logger.LogWarning("{Excepcion}: {Mensaje}", ex.GetType().Name, ex.Message);
        }

        context.Response.ContentType = "application/json";
        context.Response.StatusCode = (int)statusCode;

        var respuesta = new
        {
            status = (int)statusCode,
            error = statusCode.ToString(),
            mensajes,
            ruta = context.Request.Path.Value,
            timestamp = DateTime.UtcNow
        };

        await context.Response.WriteAsync(JsonSerializer.Serialize(respuesta));
    }
}
