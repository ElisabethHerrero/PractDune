using System;
using DomainModels.Catalogs.Escenario;
using DomainModels.Enums;

public static class EscenarioConfig
{
    // Definimos los datos base de cada escenario
    public static readonly Dictionary<EscenarioJuego, (long SolarisIniciales, string NombreEnclave, int Hectareas, int Visitantes, NivelAdquisitivo Nivel)>
    DatosEscenarios = new Dictionary<EscenarioJuego, (long, string, int, int, NivelAdquisitivo)>
    {
            {
                EscenarioJuego.Arrakeen,
                (100000, "Arrakeen", 7700, 1000, NivelAdquisitivo.Alto)
            },
            {
                EscenarioJuego.GiediPrime,
                (50000, "Giedi Prime", 100, 2000, NivelAdquisitivo.Bajo)
            },
            {
                EscenarioJuego.Caladan,
                (150000, "Caladan", 10000, 3000, NivelAdquisitivo.Medio)
            }
    };
    }