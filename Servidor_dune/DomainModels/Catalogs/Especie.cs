using System;
namespace DomainModels.Catalogs.Especie
{
    public class DatosEspecie
    {
        public Medio Medio { get; init; }
        public TipoAlimentacion Alimentacion { get; init; }
        public int EdadAdulta { get; init; }
        public int ApetitoBase { get; init; }
    }
}