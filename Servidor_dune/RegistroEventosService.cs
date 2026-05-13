using DomainModels.Entidades;
using DomainModels.Enums;
using ServidorDune.Services.Interfaces;
using System;

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
            if (partida == null)
                throw new ArgumentNullException(nameof(partida));

            RegistroEvento evento = new RegistroEvento
            {
                Id = Guid.NewGuid(),
                FechaHora = DateTime.Now,
                TipoEvento = tipoEvento,
                Descripcion = descripcion,
                Severidad = severidad,
                PartidaId = partida.Id
            };

            partida.HistorialEvento.Add(evento);
        }
    }
}