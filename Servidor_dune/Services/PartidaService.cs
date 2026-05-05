using DomainModels.Entidades;
using DomainModels.Enums;
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
                SeveridadEvento.Info);

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
                SeveridadEvento.Info);

            _persistenciaService.GuardarPartida(partida);
        }

        public Partida CargarPartida(Guid idPartida)
        {
            Partida partida = _persistenciaService.CargarPartida(idPartida);
            _partidas[idPartida] = partida;

            return partida;
        }
    }
}