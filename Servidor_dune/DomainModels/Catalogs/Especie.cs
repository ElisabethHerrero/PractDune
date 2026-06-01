using System;
namespace DomainModels.Catalogs.Especie
{

    public enum Especie
    {
        GusanoArenaJuvenil,
        TigreLaza,
        MuadDib,
        HalconDesierto,
        TruchaArena
    }

    public class Especie
    {
        public Medio Medio { get; init; }
        public TipoAlimentacion Alimentacion { get; init; }
        public int EdadAdulta { get; init; }
        public int ApetitoBase { get; init; }
    }
}