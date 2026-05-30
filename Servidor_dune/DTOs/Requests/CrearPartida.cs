using DomainModels.Enums

namespace DTOs.Requests 
{

    public class CrearPartida
    {
        public string Nombre { get; set; }
        public EscenarioJuego Escenario { get; set; }
    }
}


