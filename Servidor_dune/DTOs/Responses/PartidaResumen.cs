using DomainModels.Entidades;
using DomainModels.Enums;

namespace Dune.API.DTOs.Responses
{
    public class PartidaResumenDTO : OperacionResultado
    {
        public Guid Id { get; set; }
        public string NombreJugador { get; set; }
        public EscenarioJuego Escenario { get; set; }
        public decimal Solaris { get; set; }
        public int RondaActual { get; set; }
        public EstadoPartida EstadoPartida { get; set; }

        public PartidaResumenDTO() { }

        public PartidaResumenDTO(Partida partida)
        {
            Id = partida.Id;
            NombreJugador = partida.NombreJugador;
            Escenario = partida.Escenario;
            Solaris = partida.Solaris;
            RondaActual = partida.RondaActual;
            EstadoPartida = partida.EstadoPartida;
        }
    }
}