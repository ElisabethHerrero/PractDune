using Microsoft.AspNetCore.Mvc;
using DomainModels.Entidades;
using DomainModels.Enums;
using Dune.API.DTOs;

namespace Dune.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PartidaController : ControllerBase
    {
        // Aquí inyectarías tu servicio de persistencia/aplicación
        // private readonly IPartidaService _partidaService;

        public PartidaController()
        {
            // Constructor
        }

        [HttpPost("crear")]
        public IActionResult CrearPartida([FromBody] CrearPartidaRequest request)
        {
            try
            {
                // 1. Validar los datos de entrada
                if (string.IsNullOrWhiteSpace(request.NombreJugador))
                {
                    return BadRequest("El nombre del jugador es obligatorio.");
                }

                // 2. Usar el método de fábrica de tu dominio
                // Nota: En tu Partida.cs el método se llama InicializarNueva
                Partida nuevaPartida = Partida.InicializarNueva(request.NombreJugador, request.TipoEscenario);

                // 3. Guardar la partida usando tu servicio de persistencia
                // _partidaService.Guardar(nuevaPartida);

                // 4. Devolver respuesta a Unity (puedes devolver el ID o la partida completa)
                return Ok(new
                {
                    Mensaje = "Partida creada con éxito",
                    PartidaId = nuevaPartida.Id,
                    SolarisIniciales = nuevaPartida.Solaris
                });
            }
            catch (Exception ex)
            {
                // Manejo de errores (Sección 3.9 del documento)
                return StatusCode(500, $"Error interno al crear la partida: {ex.Message}");
            }
        }
    }
}
