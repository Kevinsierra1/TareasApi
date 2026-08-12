using TareasApi.DTOs;

namespace TareasApi.Services;


public interface ITareaService
{
    IEnumerable<TareaResponseDto> ObtenerTodas(string? estado = null);
    TareaResponseDto ObtenerPorId(int id);
    TareaResponseDto Crear(CrearTareaDto dto);
    TareaResponseDto Actualizar(int id, ActualizarTareaDto dto);
    void Eliminar(int id);
}
