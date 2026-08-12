namespace TareasApi.Models;

/// <summary>
/// Valores permitidos para el campo "estado" de una tarea.
/// </summary>
public static class EstadosTarea
{
    public const string Pendiente = "pendiente";
    public const string EnProgreso = "en_progreso";
    public const string Completada = "completada";

    public static readonly string[] Valores =
    {
        Pendiente,
        EnProgreso,
        Completada
    };

    public static bool EsValido(string estado) =>
        Valores.Contains(estado, StringComparer.OrdinalIgnoreCase);
}
