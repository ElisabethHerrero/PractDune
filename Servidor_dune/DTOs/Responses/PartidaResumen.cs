namespace DTOs.Responses
{
    public class PartidaResumen
    {
        public Guid Id { get; set; }
        public string AliasJugador { get; set; }
        public EscenarioJuego Escenario { get; set; }
        public decimal Fondos { get; set; }
        public int RondaActual { get; set; }
        public EstadoPartida EstadoPartida { get; set; }
    }
}