using System;
using System.Collections.Generic;
using DomainModels.Enums;

public class Partida
{
    public Guid Id { get; set; } //esto es mejor que un int para los id
    public string AliasJugador { get; set; }
    public EscenarioJuego Escenario { get; set; }
    public decimal Solaris { get; set; } 

    //public DateTime FechaCreacion { get; set; } //esto no es necesario

    public int RondaActual { get; set; }
    public EstadoPartida EstadoPartida { get; set; }

    public List<Enclave> Enclaves { get; set; } = new();
    public List<RegistroEvento> HistorialEvento { get; set; } = new();


    public static Partida InicializarNueva(string nombreJugador, EscenarioJuego tipoEscenario)
    {
        // 1. Obtenemos la configuración del escenario desde nuestra clase externa
        var config = EscenarioConfig.DatosEscenarios[tipoEscenario];

        var partida = new Partida
        {
            NombreJugador = nombreJugador,
            Escenario = tipoEscenario,
            Solaris = config.SolarisIniciales,
            MesActual = 1
        };

        // 2. Añadimos el enclave de exhibición según la config
        partida.Enclaves.Add(new Enclave(
            config.NombreEnclave,
            EnclaveTipo.Exhibicion,
            config.Hectareas,
            config.Visitantes,
            config.Nivel
        ));

        // 3. Añadimos el enclave de aclimatación común (Sección 3.3)
        partida.Enclaves.Add(new Enclave(
            "Cuenca Experimental",
            EnclaveTipo.Aclimatacion,
            5000,
            0,
            null
        ));

        partida.RegistrarEvento($"Partida creada en {tipoEscenario}.");
        return partida;
    }


    public void RegistrarEvento(string descripcion)
    {
        HistorialEventos.Add(new RegistroEvento
        {
            Id = Guid.NewGuid(),
            FechaHora = DateTime.Now,
            TipoEvento = tipo,
            Descripcion = desc,
            Severidad = severidad,
            PartidaId = this.Id // o de donde venga
        });
    }

}