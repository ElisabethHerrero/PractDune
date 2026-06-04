using ServidorDune.Services;
using ServidorDune.Services.Interfaces;
using Persistence; // Asegúrate de que este sea el namespace de tu persistencia

var builder = WebApplication.CreateBuilder(args);

// 1. Activar soporte para Controladores y JSON (importante para Unity)
builder.Services.AddControllers()
    .AddJsonOptions(options => {
        options.JsonSerializerOptions.PropertyNamingPolicy = null; // Mantiene nombres tal cual en C#
    });

// 2. Configurar Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// 3. REGISTRO DE TODOS LOS SERVICIOS (Evita el error en builder.Build)
// Es vital registrar CADA interfaz que uses en tus constructores
builder.Services.AddSingleton<IPersistenciaService, PersistenciaService>();
builder.Services.AddScoped<IRegistroEventosService, RegistroEventosService>();
builder.Services.AddScoped<ISimulacionService, SimulacionService>();
builder.Services.AddScoped<IPartidaService, PartidaService>();

var app = builder.Build();

// 4. Configurar el pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Dune API V1");
        c.RoutePrefix = "swagger"; // Esto hace que abra swagger al inicio
    });
}

app.UseHttpsRedirection();
app.UseAuthorization();

// 5. Mapear controladores de la carpeta Controllers
app.MapControllers();

app.Run();
