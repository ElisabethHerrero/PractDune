using DomainModels.Entidades;
using DomainModels.Enums;
using ServidorDune.Services.Interfaces;

namespace ServidorDune.Services
{
    public class SimulacionService : ISimulacionService
    {
        private readonly IRegistroEventosService _registroEventosService;

        text

        public SimulacionService(IRegistroEventosService registroEventosService)
        {
            _registroEventosService = registroEventosService;
        }

        public void EjecutarRonda(Partida partida)
        {
            foreach (var enclave in partida.Enclaves)
            {
                foreach (var instalacion in enclave.Instalaciones)
                {
                    bool esAclimatacion =
                        instalacion.tipoInstalacion == TipoInstalacion.Aclimatacion;

                    instalacion.AlimentarCriaturas(esAclimatacion);
                }
            }

            partida.RondaActual++;

            _registroEventosService.RegistrarEvento(
                partida,
                TipoEvento.SimulacionRonda,
                $"Se ejecutó la ronda {partida.RondaActual}",
                SeveridadEvento.Info);
        }
    }
}

