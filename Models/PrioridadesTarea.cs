namespace TareasApi.Models;

/// <summary>
/// Valores permitidos para el campo "prioridad" de una tarea.
/// </summary>
public static class PrioridadesTarea
{
    public const string Baja = "baja";
    public const string Media = "media";
    public const string Alta = "alta";

    public static readonly string[] Valores =
    {
        Baja,
        Media,
        Alta
    };

    public static bool EsValida(string prioridad) =>
        Valores.Contains(prioridad, StringComparer.OrdinalIgnoreCase);
}
