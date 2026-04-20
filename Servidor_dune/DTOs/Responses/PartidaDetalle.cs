namespace DTOs.Responses
{
    public class PartidaDetalle
    {
        public Guid Id { get; set; }
        public string AliasJugador { get; set; }
        public EscenarioJuego Escenario { get; set; }
        public decimal Fondos { get; set; }
        public int RondaActual { get; set; }

        public List<EnclaveDto> Enclaves { get; set; } = new();
        public List<EventoDto> EventosRecientes { get; set; } = new();

    }
}