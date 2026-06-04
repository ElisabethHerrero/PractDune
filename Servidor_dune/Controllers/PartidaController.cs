using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using ServidorDune.Services.Interfaces;
using DTOs.Requests;
using DTOs.Common;
using Dune.API.DTOs.Responses;
using DTOs.Request;
using DomainModels.Entidades;
using ServidorDune.Services;


namespace Dune.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PartidaController : ControllerBase
    {
        private readonly IPartidaService _partidaService;

        public PartidaController(IPartidaService partidaService)
        {
            _partidaService = partidaService;
        }

        /// <summary>
        /// POST: api/partida/crear
        /// Crea una nueva partida.
        /// </summary>
        [HttpPost("crear")]
        public IActionResult CrearPartida([FromBody] CrearPartida request)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(request.Nombre))
                    return BadRequest(new OperacionResultado { Success = false, Message = "El nombre del jugador es requerido." });

                var partida = _partidaService.CrearPartida(request.Nombre, request.Escenario);
                return Ok(new PartidaResumenDTO(partida) { Success = true, Message = "Partida creada exitosamente." });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new OperacionResultado { Success = false, Message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new OperacionResultado { Success = false, Message = $"Error interno al crear la partida: {ex.Message}" });
            }
        }

        /// <summary>
        /// GET: api/partida/listar
        /// Lista todas las partidas guardadas.
        /// </summary>
        [HttpGet("listar")]
        public IActionResult ListarPartidas()
        {
            try
            {
                var partidas = _partidaService.ObtenerPartidas();
                var dtos = partidas.Select(p => new PartidaResumenDTO(p)).ToList();
                return Ok(new ListaPartidasResponse { Success = true, Message = "Partidas listadas exitosamente.", Partidas = dtos });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new OperacionResultado { Success = false, Message = $"Error interno al listar partidas: {ex.Message}" });
            }
        }

        /// <summary>
        /// GET: api/partida/{id}
        /// Obtiene los detalles de una partida específica.
        /// </summary>
        [HttpGet("{id}")]
        public IActionResult ObtenerDetallePartida(Guid id)
        {
            try
            {
                var partida = _partidaService.ObtenerPartida(id);
                return Ok(new PartidaDetalleDTO(partida) { Success = true, Message = "Detalle de partida obtenido exitosamente." });
            }
            catch (KeyNotFoundException)
            {
                return NotFound(new OperacionResultado { Success = false, Message = $"Partida con ID {id} no encontrada." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new OperacionResultado { Success = false, Message = $"Error interno al obtener detalle de partida: {ex.Message}" });
            }
        }

        /// <summary>
        /// POST: api/partida/guardar/{id}
        /// Guarda el estado actual de una partida.
        /// </summary>
        [HttpPost("guardar/{id}")]
        public IActionResult GuardarPartida(Guid id)
        {
            try
            {
                _partidaService.GuardarPartida(id);
                return Ok(new OperacionResultado { Success = true, Message = $"Partida con ID {id} guardada exitosamente." });
            }
            catch (KeyNotFoundException)
            {
                return NotFound(new OperacionResultado { Success = false, Message = $"Partida con ID {id} no encontrada." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new OperacionResultado { Success = false, Message = $"Error interno al guardar partida: {ex.Message}" });
            }
        }

        /// <summary>
        /// POST: api/partida/ejecutarRonda/{id}
        /// Ejecuta una ronda de simulación para la partida especificada.
        /// </summary>
        [HttpPost("ejecutarRonda/{id}")]
        public IActionResult EjecutarRonda(Guid id)
        {
            try
            {
                var partidaActualizada = _partidaService.EjecutarRonda(id);
                return Ok(new PartidaDetalleDTO(partidaActualizada) { Success = true, Message = "Ronda ejecutada exitosamente." });
            }
            catch (KeyNotFoundException)
            {
                return NotFound(new OperacionResultado { Success = false, Message = $"Partida con ID {id} no encontrada." });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new OperacionResultado { Success = false, Message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new OperacionResultado { Success = false, Message = $"Error interno al ejecutar ronda: {ex.Message}" });
            }
        }

        /// <summary>
        /// DELETE: api/partida/eliminar/{id}
        /// Elimina una partida guardada.
        /// </summary>
        /*[HttpDelete("eliminar/{id}")]
        public IActionResult EliminarPartida(Guid id)
        {
            try
            {
                _partidaService.EliminarPartida(id);
                return Ok(new OperacionResultado { Success = true, Message = $"Partida con ID {id} eliminada exitosamente." });
            }
            catch (KeyNotFoundException)
            {
                return NotFound(new OperacionResultado { Success = false, Message = $"Partida con ID {id} no encontrada." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new OperacionResultado { Success = false, Message = $"Error interno al eliminar partida: {ex.Message}" });
            }
        }*/

        [HttpPost("construir-instalacion")]
        public IActionResult ConstruirInstalacion([FromBody] ConstruirInstalacionRequest request)
        {
            try
            {
                Instalacion nuevaInstalacion = _partidaService.ConstruirInstalacion(
                    request.PartidaId,
                    request.EnclaveId,
                    request.Codigo
                );

                return Ok(new
                {
                    success = true,
                    message = $"Instalación {nuevaInstalacion.tipoInstalacion} construida con éxito.",
                    instalacionId = nuevaInstalacion.Codigo, // O Id si usas Guid
                    solarisRestantes = _partidaService.ObtenerPartida(request.PartidaId).Solaris
                });
            }
            catch (ArgumentException ex)
            {
                return NotFound(new { success = false, error = ex.Message });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { success = false, error = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, error = $"Error interno al construir instalación: {ex.Message}" });
            }
        }
    }
}