namespace TareasApi.DTOs;

/// <summary>
/// Representación de una tarea devuelta por la API.
/// </summary>
public class TareaResponseDto
{
    public int Id { get; set; }
    public string Titulo { get; set; } = string.Empty;
    public string? Descripcion { get; set; }
    public string Estado { get; set; } = string.Empty;
    public string Prioridad { get; set; } = string.Empty;
    public DateTime FechaCreacion { get; set; }
    public string? Responsable { get; set; } = string.Empty;
}
