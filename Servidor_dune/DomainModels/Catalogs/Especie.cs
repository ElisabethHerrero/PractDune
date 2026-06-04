using System;
using DomainModels.Enums;

namespace DomainModels.Catalogs.Especie
{

    public enum TipoEspecie
    {
        GusanoArenaJuvenil,
        TigreLaza,
        MuadDib,
        HalconDesierto,
        TruchaArena
    }

    public class Especie
    {
        public TipoEspecie especie { get; init; }
        public Medio Medio { get; init; }
        public TipoAlimentacion Alimentacion { get; init; }
        public int EdadAdulta { get; init; }
        public int ApetitoBase { get; init; }
    }
}