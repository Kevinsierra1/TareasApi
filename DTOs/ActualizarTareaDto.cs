using System.ComponentModel.DataAnnotations;

namespace TareasApi.DTOs;

/// <summary>
/// Datos de entrada para actualizar una tarea existente (reemplazo completo).
/// </summary>
public class ActualizarTareaDto
{
    [Required(ErrorMessage = "El título es obligatorio.")]
    [MaxLength(200, ErrorMessage = "El título no puede superar los 200 caracteres.")]
    public string Titulo { get; set; } = string.Empty;

    public string? Descripcion { get; set; }

    [Required(ErrorMessage = "El estado es obligatorio.")]
    public string Estado { get; set; } = string.Empty;

    [Required(ErrorMessage = "La prioridad es obligatoria.")]
    public string Prioridad { get; set; } = string.Empty;

    [Required(ErrorMessage = "El responsable es obligatorio.")]
    public string? Responsable { get; set; }
}
