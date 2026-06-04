using DomainModels.Catalogs.Especie;
using DomainModels.Enums;

namespace DTOs.COmmon
{
    public class CriaturaDTO
    {
        public Guid Id { get; set; }
        public Especie especie { get; set; }

        public int EdadActual { get; set; }
        public int Salud { get; set; }

        public int EdadAdulta { get; set; }

        public Medio Medio { get; set; }
        public TipoAlimentacion TipoAlimentacion { get; set; }

        public int VecesFavorita { get; set; }

        public EstadoCriatura EstadoCriatura { get; set; }
    }

}
