using TareasApi.Middleware;
using TareasApi.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(options =>
{
    options.AddPolicy("PermitirReact", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(opciones =>
{
    opciones.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
    {
        Title = "Tareas API",
        Version = "v1",
        Description = "API REST para gestión de tareas (to-do) con persistencia en memoria."
    });
});

builder.Services.AddSingleton<ITareaService, TareaService>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(opciones =>
    {
        opciones.SwaggerEndpoint("/swagger/v1/swagger.json", "Tareas API v1");
    });
}

app.UseMiddleware<ExceptionMiddleware>();

app.UseCors("PermitirReact");

app.UseHttpsRedirection();
app.MapControllers();

app.Run();

public partial class Program { }