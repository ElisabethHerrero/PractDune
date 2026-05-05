using DomainModels.Entidades;
using DomainModels.Enums;
using ServidorDune.Services.Interfaces;
using System;
using static System.Net.Mime.MediaTypeNames;

namespace ServidorDune.Services
{
    public class RegistroEventosService : IRegistroEventosService
    {
        public void RegistrarEvento(
        Partida partida,
        TipoEvento tipoEvento,
        string descripcion,
        SeveridadEvento severidad)
        {
            RegistroEvento evento = new RegistroEvento
            {
                Id = Guid.NewGuid(),
                FechaHora = DateTime.Now,
                TipoEvento = tipoEvento,
                Descripcion = descripcion,
                Severidad = severidad,
                PartidaId = partida.Id
            };

            text
        
        partida.HistorialEvento.Add(evento);
        }
    }
}

