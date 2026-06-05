using DomainModels.Entidades;
using DomainModels.Enums;
using InstalacionesConf;
using ServidorDune.Services.Interfaces;
using System;
using System.Collections.Generic;

namespace ServidorDune.Services
{
    public class PartidaService : IPartidaService
    {
        private readonly IPersistenciaService _persistenciaService;
        private readonly ISimulacionService _simulacionService;
        private readonly IRegistroEventosService _registroEventosService;

        private readonly Dictionary<Guid, Partida> _partidas = new();

        public PartidaService(
            IPersistenciaService persistenciaService,
            ISimulacionService simulacionService,
            IRegistroEventosService registroEventosService)
        {
            _persistenciaService = persistenciaService;
            _simulacionService = simulacionService;
            _registroEventosService = registroEventosService;

            foreach (Partida partida in _persistenciaService.CargarTodasLasPartidas())
            {
                _partidas[partida.Id] = partida;
            }
        }

        public Partida CrearPartida(string aliasJugador, EscenarioJuego escenario)
        {
            if (string.IsNullOrWhiteSpace(aliasJugador))
                throw new ArgumentException("El alias del jugador es obligatorio.");

            Partida partida = Partida.InicializarNueva(aliasJugador, escenario);

            _registroEventosService.RegistrarEvento(
                partida,
                TipoEvento.CreacionPartida,
                $"Partida creada para {aliasJugador} en el escenario {escenario}.",
                Severidad.Info);

            _partidas[partida.Id] = partida;
            _persistenciaService.GuardarPartida(partida);

            return partida;
        }

        public Partida ObtenerPartida(Guid idPartida)
        {
            if (_partidas.TryGetValue(idPartida, out Partida? partida))
                return partida;

            partida = _persistenciaService.CargarPartida(idPartida);
            _partidas[idPartida] = partida;

            return partida;
        }

        public List<Partida> ObtenerPartidas()
        {
            return _partidas.Values.ToList();
        }

        public Partida EjecutarRonda(Guid idPartida)
        {
            Partida partida = ObtenerPartida(idPartida);

            _simulacionService.EjecutarRonda(partida);
            _persistenciaService.GuardarPartida(partida);

            return partida;
        }

        public void GuardarPartida(Guid idPartida)
        {
            Partida partida = ObtenerPartida(idPartida);

            _registroEventosService.RegistrarEvento(
                partida,
                TipoEvento.GuardadoPartida,
                "Partida guardada correctamente.",
                Severidad.Info);

            _persistenciaService.GuardarPartida(partida);
        }

        public Partida CargarPartida(Guid idPartida)
        {
            Partida partida = _persistenciaService.CargarPartida(idPartida);
            _partidas[idPartida] = partida;

            return partida;
        }




        public Instalacion ConstruirInstalacion(Guid partidaId, Guid enclaveId, string codigoInstalacion)
        {
            // 1. Obtener la partida y el enclave
            Partida partida = ObtenerPartida(partidaId);
            Enclave enclave = partida.Enclaves.FirstOrDefault(e => e.Id == enclaveId);

            if (enclave == null)
                throw new ArgumentException($"Enclave no encontrado.");

            // 2. Buscar la "plantilla" en el catálogo
            if (!InstalacionesCatalogo.Todas.TryGetValue(codigoInstalacion, out var plantilla))
                throw new ArgumentException($"El código {codigoInstalacion} no existe en el catálogo.");

            // 3. Validar si el jugador tiene suficiente dinero
            if (partida.Solaris < plantilla.Coste)
                throw new InvalidOperationException("Solaris insuficientes.");

            // 4. Crear la nueva instancia basada en la plantilla
            // Usamos el constructor con los datos de la plantilla
            Instalacion nuevaInstalacion = new Instalacion(
                plantilla.Id,
                plantilla.tipoInstalacion,
                plantilla.Codigo,
                plantilla.Coste,
                plantilla.Medio,
                plantilla.Alimentacion,
                plantilla.Capacidad,
                plantilla.Hectareas,
                plantilla.Suministros, // Aquí se aplican los suministros iniciales de tu tabla
                plantilla.TipoRecinto
            )
            {
                Id = Guid.NewGuid() // Asignamos un ID único a esta nueva construcción
            };

            // 5. Descontar dinero y añadir al enclave
            partida.Solaris -= nuevaInstalacion.Coste;
            enclave.Instalaciones.Add(nuevaInstalacion);

            // 6. Registrar evento y guardar
            _registroEventosService.RegistrarEvento(
                partida,
                TipoEvento.ConstruccionInstalacion,
                $"Construida {nuevaInstalacion.Codigo} en {enclave.Nombre}. Coste: {nuevaInstalacion.Coste}", Severidad.Info);

            _persistenciaService.GuardarPartida(partida);

            return nuevaInstalacion;
        }



        // Método auxiliar para obtener el coste (debería ser más robusto en un sistema real)

        int ObtenerCosteInstalacion(TipoRecinto tipoInstalacion)
        {
            switch (tipoInstalacion)
            {
                case TipoRecinto.RocaSellada:
                    return 1000;
                case TipoRecinto.EscudoEstático:
                    return 2500;
                case TipoRecinto.CupulaBlindada:
                    return 5000;
                case TipoRecinto.PozoReforzado:
                    return 3500;
                default:
                    return 1000; // Coste por defecto
            }
        }
        





    }
}