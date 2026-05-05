using System;

namespace DomainModels.Entidades
{
    public class RegistroEvento
    {
        public Guid Id { get; set; }
        public DateTime FechaHora { get; set; }

        public TipoEvento TipoEvento { get; set; }
        public string Descripcion { get; set; }
        public SeveridadEvento Severidad { get; set; }

        public Guid PartidaId { get; set; }
    }

}