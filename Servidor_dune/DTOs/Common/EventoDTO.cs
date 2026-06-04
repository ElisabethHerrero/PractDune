using DomainModels.Enums;

namespace DTOs.Common
{
    public class EventoDto
    {
        public int Id { get; set; }
        public DateTime FechaHora { get; set; }
        public TipoEvento TipoEvento { get; set; }
        public string Descripcion { get; set; }
        public Severidad Severidad { get; set; }
    }
}