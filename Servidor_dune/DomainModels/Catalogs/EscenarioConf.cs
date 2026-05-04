using System;
using DomainModels.Catalogs.Escenario;
using DomainModels.Enums;

public static class EscenarioConfig
{
    // Definimos los datos base de cada escenario
    public static readonly Dictionary<EscenarioTipo, (long SolarisIniciales, string NombreEnclave, double Hectareas, int Visitantes, NivelAdquisitivo Nivel)>
    DatosEscenarios = new Dictionary<EscenarioTipo, (long, string, double, int, NivelAdquisitivo)>
    {
            {
                EscenarioTipo.Arrakeen,
                (100000, "Arrakeen", 7700, 1000, NivelAdquisitivo.ALTO)
            },
            {
                EscenarioTipo.GiediPrime,
                (50000, "Giedi Prime", 100, 2000, NivelAdquisitivo.BAJO)
            },
            {
                EscenarioTipo.Caladan,
                (150000, "Caladan", 10000, 3000, NivelAdquisitivo.MEDIO)
            }
    };