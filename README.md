# Tareas API

API REST en **C# / .NET 9 (ASP.NET Core Web API)** para gestionar tareas (to-do), con persistencia en memoria y arquitectura por capas.

## Cómo ejecutarla

```bash
cd TareasApi
dotnet restore
dotnet run
```

Por defecto queda disponible en `http://localhost:5080`. En entorno `Development` se abre Swagger automáticamente en `/swagger`.

También puedes usar el archivo `TareasApi.http` (compatible con la extensión REST Client de VS Code, Visual Studio o Rider) para probar todos los endpoints sin salir del editor.

## Arquitectura

Separación en capas, siguiendo el mismo espíritu que AutoTallerManager pero a menor escala:

```
Controllers/   -> Solo HTTP: recibe la petición, delega y devuelve el código de estado correcto
Services/      -> Reglas de negocio y validación (ITareaService / TareaService)
Models/        -> Entidad de dominio Tarea + constantes de Estado/Prioridad
DTOs/          -> Contratos de entrada/salida (CrearTareaDto, ActualizarTareaDto, TareaResponseDto)
Exceptions/    -> Excepciones de dominio (AppValidationException, BusinessRuleException, NotFoundException)
Middleware/    -> ExceptionMiddleware: manejo de errores centralizado
```

`TareaService` se registra como **Singleton** (no Scoped) porque la lista en memoria debe sobrevivir entre peticiones HTTP; el acceso está protegido con `lock` por si llegan peticiones concurrentes.

## Endpoints

| Método | Ruta                      | Descripción                              |
|--------|---------------------------|-------------------------------------------|
| GET    | `/tareas`                 | Lista todas las tareas                    |
| GET    | `/tareas?estado=pendiente`| Filtra tareas por estado                  |
| GET    | `/tareas/{id}`            | Obtiene una tarea por id                  |
| POST   | `/tareas`                 | Crea una tarea                            |
| PUT    | `/tareas/{id}`            | Actualiza una tarea (reemplazo completo)  |
| DELETE | `/tareas/{id}`            | Elimina una tarea                         |

## Reglas implementadas

- **`titulo`** obligatorio (400 si falta o va vacío).
- **`estado`** solo acepta `pendiente`, `en_progreso`, `completada` (400 si no).
- **`prioridad`** solo acepta `baja`, `media`, `alta` (400 si no).
- **Regla de negocio**: no se puede guardar una tarea con `estado = completada` si `descripcion` está vacía o ausente (400).
- `id` y `fechaCreacion` se asignan automáticamente y no pueden enviarse desde el cliente.
- Los valores de `estado`/`prioridad` se normalizan a minúsculas antes de validarse, para tolerar `"Pendiente"`, `"PENDIENTE"`, etc.

## Manejo de errores centralizado

`ExceptionMiddleware` intercepta cualquier excepción del pipeline y devuelve una respuesta JSON uniforme:

```json
{
  "status": 400,
  "error": "BadRequest",
  "mensajes": ["No se puede marcar una tarea como 'completada' si no tiene descripción."],
  "ruta": "/tareas",
  "timestamp": "2026-08-12T12:00:00Z"
}
```

Mapeo de excepciones -> código HTTP:

| Excepción                | Código |
|---------------------------|--------|
| `AppValidationException`  | 400    |
| `BusinessRuleException`   | 400    |
| `NotFoundException`       | 404    |
| Cualquier otra excepción  | 500    |

Los `[Required]`/`[MaxLength]` de los DTOs también generan un 400 automático (vía `[ApiController]`) antes de que la petición llegue al servicio.

## Posibles siguientes pasos

- Persistencia real con EF Core + PostgreSQL o SQLite (reemplazando solo `TareaService` por una implementación con `DbContext`, gracias a la interfaz `ITareaService`).
- Paginación en `GET /tareas`.
- Pruebas unitarias con xUnit + Moq sobre `TareaService` y de integración con `WebApplicationFactory`.
