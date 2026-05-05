using ServidorDune.Services;
using ServidorDune.Services.Interfaces;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// REGISTRO DE SERVICIOS

builder.Services.AddSingleton<IPersistenciaService, PersistenciaService>();

builder.Services.AddSingleton<IRegistroEventosService, RegistroEventosService>();

builder.Services.AddSingleton<ISimulacionService, SimulacionService>();

builder.Services.AddSingleton<IPartidaService, PartidaService>();

var app = builder.Build();


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();