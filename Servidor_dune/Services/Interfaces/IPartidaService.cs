using DomainModels.Entidades;
using DomainModels.Enums;
using System;
using System.Collections.Generic;

namespace ServidorDune.Services.Interfaces
{
    public interface IPartidaService
    {
        Partida CrearPartida(string aliasJugador, EscenarioJuego escenario);
        Partida ObtenerPartida(Guid idPartida);
        List<Partida> ObtenerPartidas();
        Partida EjecutarRonda(Guid idPartida);
        void GuardarPartida(Guid idPartida);
        Partida CargarPartida(Guid idPartida);
    }
}