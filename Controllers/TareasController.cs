using Microsoft.AspNetCore.Mvc;
using TareasApi.DTOs;
using TareasApi.Services;

namespace TareasApi.Controllers;

/// <summary>
/// Expone los endpoints CRUD para la gestión de tareas.
/// La capa de controlador solo se encarga de HTTP: delega toda
/// la lógica y validación de negocio en <see cref="ITareaService"/>.
/// </summary>
[ApiController]
[Route("tareas")]
[Produces("application/json")]
public class TareasController : ControllerBase
{
    private readonly ITareaService _tareaService;

    public TareasController(ITareaService tareaService)
    {
        _tareaService = tareaService;
    }

    /// <summary>Lista todas las tareas. Admite filtrado opcional por estado.</summary>
    /// <remarks>GET /tareas o GET /tareas?estado=pendiente</remarks>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<TareaResponseDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public ActionResult<IEnumerable<TareaResponseDto>> ObtenerTodas([FromQuery] string? estado)
    {
        var tareas = _tareaService.ObtenerTodas(estado);
        return Ok(tareas);
    }

    /// <summary>Obtiene una tarea por su id.</summary>
    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(TareaResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public ActionResult<TareaResponseDto> ObtenerPorId(int id)
    {
        var tarea = _tareaService.ObtenerPorId(id);
        return Ok(tarea);
    }

    /// <summary>Crea una nueva tarea.</summary>
    [HttpPost]
    [ProducesResponseType(typeof(TareaResponseDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public ActionResult<TareaResponseDto> Crear([FromBody] CrearTareaDto dto)
    {
        var creada = _tareaService.Crear(dto);
        return CreatedAtAction(nameof(ObtenerPorId), new { id = creada.Id }, creada);
    }

    /// <summary>Actualiza una tarea existente (reemplazo completo).</summary>
    [HttpPut("{id:int}")]
    [ProducesResponseType(typeof(TareaResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public ActionResult<TareaResponseDto> Actualizar(int id, [FromBody] ActualizarTareaDto dto)
    {
        var actualizada = _tareaService.Actualizar(id, dto);
        return Ok(actualizada);
    }

    /// <summary>Elimina una tarea por su id.</summary>
    [HttpDelete("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public IActionResult Eliminar(int id)
    {
        _tareaService.Eliminar(id);
        return NoContent();
    }
}
