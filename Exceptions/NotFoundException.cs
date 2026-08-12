namespace TareasApi.Exceptions;

/// <summary>
/// Se lanza cuando el recurso solicitado no existe.
/// El middleware de errores la traduce a HTTP 404.
/// </summary>
public class NotFoundException : Exception
{
    public NotFoundException(string message) : base(message)
    {
    }
}
