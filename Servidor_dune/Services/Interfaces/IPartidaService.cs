using DomainModels.Entidades;
using System;

namespace ServidorDune.Services.Interfaces
{
    public interface IPersistenciaService
    {
        void GuardarPartida(Partida partida);
        Partida CargarPartida(Guid id);
    }
}

