namespace TareasApi.Exceptions;

/// <summary>
/// Se lanza cuando una operación viola una regla de negocio
/// (ej. marcar como "completada" una tarea sin descripción).
/// El middleware de errores la traduce a HTTP 422.
/// </summary>
public class BusinessRuleException : Exception
{
    public BusinessRuleException(string message) : base(message)
    {
    }
}
