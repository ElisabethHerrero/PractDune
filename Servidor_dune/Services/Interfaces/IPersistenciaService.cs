using DomainModels.Entidades;
using System;
using System.Collections.Generic;

namespace ServidorDune.Services.Interfaces
{
    public interface IPersistenciaService
    {
        void GuardarPartida(Partida partida);
        Partida CargarPartida(Guid idPartida);
        List<Partida> CargarTodasLasPartidas();
    }
}