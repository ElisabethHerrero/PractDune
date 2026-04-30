using System;
using DomainModels.Catalogs.Especie;

public static class EspecieConfig
{
    // Diccionario que mapea cada Especie con sus datos técnicos
    private static readonly Dictionary<Especie, DatosEspecie> _configuraciones = new()
    {
        {
            Especie.GusanoArenaJuvenil,
            new DatosEspecie { Medio = Medio.Subterraneo, Alimentacion = TipoAlimentacion.Depredador, EdadAdulta = 24, ApetitoBase = 5 }
        },
        {
            Especie.TigreLaza,
            new DatosEspecie { Medio = Medio.Desierto, Alimentacion = TipoAlimentacion.Depredador, EdadAdulta = 38, ApetitoBase = 8 }
        },
        {
            Especie.MuadDib,
            new DatosEspecie { Medio = Medio.Desierto, Alimentacion = TipoAlimentacion.Recolector, EdadAdulta = 12, ApetitoBase = 2 }
        },
        {
            Especie.HalconDesierto,
            new DatosEspecie { Medio = Medio.Aereo, Alimentacion = TipoAlimentacion.Depredador, EdadAdulta = 16, ApetitoBase = 2 }
        },
        {
            Especie.TruchaArena,
            new DatosEspecie { Medio = Medio.Subterraneo, Alimentacion = TipoAlimentacion.Recolector, EdadAdulta = 42, ApetitoBase = 10 }
        }
    };

    // Método que obtiene los datos directamente del diccionario (O(1) de complejidad)
    public static DatosEspecie Obtener(Especie especie)
    {
        if (_configuraciones.TryGetValue(especie, out var datos))
        {
            return datos;
        }
        throw new KeyNotFoundException($"La especie {especie} no está configurada.");
    }
}