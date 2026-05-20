using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using DomainModels;
using DomainModels.Enums;
using Persistence;

namespace DuneApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PartidaController : ControllerBase
    {
        private readonly GestorPersistencia _gestor;

        public PartidaController()
        {
            // Inicializar el gestor apuntando a la carpeta DATA
            string dataPath = Path.Combine(Directory.GetCurrentDirectory(), "DATA");
            _gestor = new GestorPersistencia(dataPath);
        }

        /// <summary>
        /// POST: api/partida/crear
        /// Crea una nueva partida y la guarda en el disco
        /// </summary>
        [HttpPost("crear")]
        public IActionResult CrearPartida([FromBody] CrearPartidaRequest request)
        {
            try
            {
                if (string.IsNullOrEmpty(request.NombreJugador))
                    return BadRequest("El nombre del jugador es requerido");

                // Crear la partida usando el método estático
                Partida nuevaPartida = Partida.InicializarNueva(
                    request.NombreJugador,
                    request.Escenario
                );

                // Guardar la partida en el disco
                _gestor.GuardarPartida(nuevaPartida);

                // Devolver la partida creada al cliente (Unity)
                return Ok(new
                {
                    success = true,
                    message = "Partida creada exitosamente",
                    partidaId = nuevaPartida.Id,
                    aliasJugador = nuevaPartida.AliasJugador,
                    solaris = nuevaPartida.Solaris,
                    enclaves = nuevaPartida.Enclaves.Count
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, error = ex.Message });
            }
        }

        /// <summary>
        /// GET: api/partida/cargar/{id}
        /// Carga una partida guardada desde el disco
        /// </summary>
        [HttpGet("cargar/{id}")]
        public IActionResult CargarPartida(Guid id)
        {
            try
            {
                Partida partida = _gestor.CargarPartida(id);

                return Ok(new
                {
                    success = true,
                    message = "Partida cargada exitosamente",
                    partida = partida
                });
            }
            catch (FileNotFoundException)
            {
                return NotFound(new { success = false, error = $"No se encontró la partida con ID {id}" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, error = ex.Message });
            }
        }

        /// <summary>
        /// POST: api/partida/guardar
        /// Guarda una partida existente (actualización)
        /// </summary>
        [HttpPost("guardar")]
        public IActionResult GuardarPartida([FromBody] Partida partida)
        {
            try
            {
                if (partida == null || partida.Id == Guid.Empty)
                    return BadRequest("La partida debe tener un ID válido");

                _gestor.GuardarPartida(partida);

                return Ok(new
                {
                    success = true,
                    message = "Partida guardada exitosamente",
                    partidaId = partida.Id
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, error = ex.Message });
            }
        }

        /// <summary>
        /// GET: api/partida/listar
        /// Lista todas las partidas guardadas
        /// </summary>
        [HttpGet("listar")]
        public IActionResult ListarPartidas()
        {
            try
            {
                List<Guid> partidas = _gestor.ListarPartidasGuardadas();
                return Ok(new
                {
                    success = true,
                    cantidad = partidas.Count,
                    partidas = partidas
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, error = ex.Message });
            }
        }

        /// <summary>
        /// DELETE: api/partida/eliminar/{id}
        /// Elimina una partida guardada
        /// </summary>
        [HttpDelete("eliminar/{id}")]
        public IActionResult EliminarPartida(Guid id)
        {
            try
            {
                string dataPath = Path.Combine(Directory.GetCurrentDirectory(), "DATA");
                string filePath = Path.Combine(dataPath, $"Partida_{id}.json");

                if (!System.IO.File.Exists(filePath))
                    return NotFound(new { success = false, error = "Partida no encontrada" });

                System.IO.File.Delete(filePath);
                return Ok(new { success = true, message = "Partida eliminada exitosamente" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, error = ex.Message });
            }
        }
    }

    // Clase para recibir los datos del cliente
    public class CrearPartidaRequest
    {
        public string NombreJugador { get; set; }
        public EscenarioJuego Escenario { get; set; }
    }
}
