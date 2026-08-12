namespace TareasApi.Models;

/// <summary>
/// Entidad de dominio que representa una tarea (to-do).
/// </summary>
public class Tarea
{
    public int Id { get; set; }
    public string Titulo { get; set; } = string.Empty;
    public string? Descripcion { get; set; }
    public string Estado { get; set; } = EstadosTarea.Pendiente;
    public string Prioridad { get; set; } = PrioridadesTarea.Media;
    public DateTime FechaCreacion { get; set; }
    public string? Responsable { get; set; } = string.Empty;

}
