using System;
using System.Collections.Generic;
using 

public class Partida
{
    public Guid Id { get; set; } //esto es mejor que un int para los id
    public string AliasJugador { get; set; }
    public string EscenarioSeleccionado { get; set; }
    public decimal Fondos { get; set; }
    public DateTime FechaCreacion { get; set; }
    public int RondaActual { get; set; }
    public EstadoPartida EstadoPartida { get; set; }

    public List<Enclave> Enclaves { get; set; } = new();
    public List<RegistroEvento> RegistroEventos { get; set; } = new();
}