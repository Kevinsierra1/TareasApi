namespace TareasApi.Exceptions;

/// <summary>
/// Se lanza cuando los datos de entrada no cumplen las reglas de validación.
/// El middleware de errores la traduce a HTTP 400.
/// </summary>
public class AppValidationException : Exception
{
    public IReadOnlyList<string> Errores { get; }

    public AppValidationException(string error) : base(error)
    {
        Errores = new List<string> { error };
    }

    public AppValidationException(IEnumerable<string> errores) : base(string.Join(" ", errores))
    {
        Errores = errores.ToList();
    }
}
