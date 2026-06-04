using System;
using DomainModels.Catalogs.Especie;
using DomainModels.Enums;

public static class EspecieConfig
{
    // Diccionario que mapea cada Especie con sus datos técnicos
    private static readonly Dictionary<TipoEspecie, Especie> _configuraciones = new()
    {
        {
            TipoEspecie.GusanoArenaJuvenil,
            new Especie { Medio = Medio.Subterraneo, Alimentacion = TipoAlimentacion.Depredador, EdadAdulta = 24, ApetitoBase = 5 }
        },
        {
            TipoEspecie.TigreLaza,
            new Especie { Medio = Medio.Desierto, Alimentacion = TipoAlimentacion.Depredador, EdadAdulta = 38, ApetitoBase = 8 }
        },
        {
            TipoEspecie.MuadDib,
            new Especie { Medio = Medio.Desierto, Alimentacion = TipoAlimentacion.Recolector, EdadAdulta = 12, ApetitoBase = 2 }
        },
        {
            TipoEspecie.HalconDesierto,
            new Especie { Medio = Medio.Aereo, Alimentacion = TipoAlimentacion.Depredador, EdadAdulta = 16, ApetitoBase = 2 }
        },
        {
            TipoEspecie.TruchaArena,
            new Especie { Medio = Medio.Subterraneo, Alimentacion = TipoAlimentacion.Recolector, EdadAdulta = 42, ApetitoBase = 10 }
        }
    };

    // Método que obtiene los datos directamente del diccionario (O(1) de complejidad)
    public static Especie Obtener(TipoEspecie especie)
    {
        if (_configuraciones.TryGetValue(especie, out var datos))
        {
            return datos;
        }
        throw new KeyNotFoundException($"La especie {especie} no está configurada.");
    }
        
        
 }