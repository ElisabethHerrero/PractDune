using DomainModels.Entidades;
using DomainModels.Enums;
using ServidorDune.Services.Interfaces;
using System;

namespace ServidorDune.Services
{
    public class SimulacionService : ISimulacionService
    {
        private readonly IRegistroEventosService _registroEventosService;

        public SimulacionService(IRegistroEventosService registroEventosService)
        {
            _registroEventosService = registroEventosService;
        }

        public void EjecutarRonda(Partida partida)
        {
            if (partida == null)
                throw new ArgumentNullException(nameof(partida));

            if (partida.EstadoPartida == EstadoPartida.Finalizada)
                throw new InvalidOperationException("No se puede ejecutar una ronda en una partida finalizada.");

            foreach (Enclave enclave in partida.Enclaves)
            {
                bool esAclimatacion = enclave.TipoEnclave == TipoEnclave.Aclimatacion;

                foreach (Instalacion instalacion in enclave.Instalaciones)
                {
                    instalacion.AlimentarCriaturas(esAclimatacion);
                }
            }

            partida.RondaActual++;

            if (partida.EstadoPartida == EstadoPartida.Creada)
                partida.EstadoPartida = EstadoPartida.EnCurso;

            _registroEventosService.RegistrarEvento(
                partida,
                TipoEvento.SimulacionRonda,
                $"Se ejecutó la ronda {partida.RondaActual}.",
                SeveridadEvento.Info);
        }
    }
}