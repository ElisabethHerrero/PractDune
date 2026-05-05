using DomainModels.Entidades;

namespace ServidorDune.Services.Interfaces
{
    public interface ISimulacionService
    {
        void EjecutarRonda(Partida partida);
    }
}