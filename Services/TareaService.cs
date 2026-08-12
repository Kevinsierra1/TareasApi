using TareasApi.DTOs;
using TareasApi.Exceptions;
using TareasApi.Models;

namespace TareasApi.Services;


public class TareaService : ITareaService
{
    private readonly List<Tarea> _tareas = new();
    private readonly object _bloqueo = new();
    private int _siguienteId = 1;

    public IEnumerable<TareaResponseDto> ObtenerTodas(string? estado = null)
    {
        lock (_bloqueo)
        {
            IEnumerable<Tarea> resultado = _tareas;

            if (!string.IsNullOrWhiteSpace(estado))
            {
                var estadoNormalizado = estado.Trim().ToLowerInvariant();
                ValidarEstado(estadoNormalizado);
                resultado = resultado.Where(t => t.Estado == estadoNormalizado);
            }

            return resultado
                .OrderBy(t => t.Id)
                .Select(MapearADto)
                .ToList();
        }
    }

    public TareaResponseDto ObtenerPorId(int id)
    {
        lock (_bloqueo)
        {
            var tarea = BuscarOFallar(id);
            return MapearADto(tarea);
        }
    }

    public TareaResponseDto Crear(CrearTareaDto dto)
    {
        var titulo = dto.Titulo?.Trim();
        if (string.IsNullOrWhiteSpace(titulo))
        {
            throw new AppValidationException("El título es obligatorio.");
        }

        var estado = NormalizarOPorDefecto(dto.Estado, EstadosTarea.Pendiente);
        var prioridad = NormalizarOPorDefecto(dto.Prioridad, PrioridadesTarea.Media);

        ValidarEstado(estado);
        ValidarPrioridad(prioridad);
        ValidarReglaCompletadaRequiereDescripcion(estado, dto.Descripcion);

        lock (_bloqueo)
        {
            var tarea = new Tarea
            {
                Id = _siguienteId++,
                Titulo = titulo,
                Descripcion = dto.Descripcion?.Trim(),
                Estado = estado,
                Prioridad = prioridad,
                FechaCreacion = DateTime.UtcNow
            };

            _tareas.Add(tarea);
            return MapearADto(tarea);
        }
    }

    public TareaResponseDto Actualizar(int id, ActualizarTareaDto dto)
    {
        var titulo = dto.Titulo?.Trim();
        if (string.IsNullOrWhiteSpace(titulo))
        {
            throw new AppValidationException("El título es obligatorio.");
        }

        var estado = (dto.Estado ?? string.Empty).Trim().ToLowerInvariant();
        var prioridad = (dto.Prioridad ?? string.Empty).Trim().ToLowerInvariant();

        ValidarEstado(estado);
        ValidarPrioridad(prioridad);
        ValidarReglaCompletadaRequiereDescripcion(estado, dto.Descripcion);

        lock (_bloqueo)
        {
            var tarea = BuscarOFallar(id);

            tarea.Titulo = titulo;
            tarea.Descripcion = dto.Descripcion?.Trim();
            tarea.Estado = estado;
            tarea.Prioridad = prioridad;

            return MapearADto(tarea);
        }
    }

    public void Eliminar(int id)
    {
        lock (_bloqueo)
        {
            var tarea = BuscarOFallar(id);
            _tareas.Remove(tarea);
        }
    }

    private Tarea BuscarOFallar(int id)
    {
        var tarea = _tareas.FirstOrDefault(t => t.Id == id);
        if (tarea is null)
        {
            throw new NotFoundException($"No se encontró una tarea con id {id}.");
        }

        return tarea;
    }

    private static string NormalizarOPorDefecto(string? valor, string valorPorDefecto)
    {
        return string.IsNullOrWhiteSpace(valor)
            ? valorPorDefecto
            : valor.Trim().ToLowerInvariant();
    }

    private static void ValidarEstado(string estado)
    {
        if (!EstadosTarea.EsValido(estado))
        {
            throw new AppValidationException(
                $"El estado '{estado}' no es válido. Valores permitidos: {string.Join(", ", EstadosTarea.Valores)}.");
        }
    }

    private static void ValidarPrioridad(string prioridad)
    {
        if (!PrioridadesTarea.EsValida(prioridad))
        {
            throw new AppValidationException(
                $"La prioridad '{prioridad}' no es válida. Valores permitidos: {string.Join(", ", PrioridadesTarea.Valores)}.");
        }
    }

    private static void ValidarReglaCompletadaRequiereDescripcion(string estado, string? descripcion)
    {
        if (estado == EstadosTarea.Completada && string.IsNullOrWhiteSpace(descripcion))
        {
            throw new BusinessRuleException(
                "No se puede marcar una tarea como 'completada' si no tiene descripción.");
        }
    }

    private static TareaResponseDto MapearADto(Tarea tarea) => new()
    {
        Id = tarea.Id,
        Titulo = tarea.Titulo,
        Descripcion = tarea.Descripcion,
        Estado = tarea.Estado,
        Prioridad = tarea.Prioridad,
        FechaCreacion = tarea.FechaCreacion,
        Responsable = tarea.Responsable,
    };
}
