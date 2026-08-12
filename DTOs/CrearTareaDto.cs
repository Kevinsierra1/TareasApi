using System.ComponentModel.DataAnnotations;
using TareasApi.Models;

namespace TareasApi.DTOs;

/// <summary>
/// Datos de entrada para crear una nueva tarea.
/// </summary>
public class CrearTareaDto
{
    [Required(ErrorMessage = "El título es obligatorio.")]
    [MaxLength(200, ErrorMessage = "El título no puede superar los 200 caracteres.")]
    public string Titulo { get; set; } = string.Empty;

    public string? Descripcion { get; set; }

    /// <summary>Si no se envía, se asigna "pendiente" por defecto.</summary>
    public string? Estado { get; set; } = EstadosTarea.Pendiente;

    /// <summary>Si no se envía, se asigna "media" por defecto.</summary>
    public string? Prioridad { get; set; } = PrioridadesTarea.Media;

    public string? Responsable { get; set; } = string.Empty;
}
