using DomainModels.Entidades;
using DomainModels.Enums;

namespace InstalacionesConf
{
    
    public static class InstalacionesCatalogo
    {
        public static readonly Dictionary<string, Instalacion> Todas = new()
    {
        // ---------------- Aclimatación ----------------

        {
            "ADR05",
            new Instalacion(
                TipoInstalacion.Aclimatacion,
                "ADR05",
                1000,
                Medio.Desierto,
                TipoAlimentacion.Recolector,
                5,
                200,
                0,
                TipoRecinto.RocaSellada
            )
        },

        {
            "ADP03",
            new Instalacion(
                TipoInstalacion.Aclimatacion,
                "ADP03",
                2500,
                Medio.Desierto,
                TipoAlimentacion.Depredador,
                3,
                300,
                10,
                TipoRecinto.EscudoEstático
            )
        },

        {
            "AAV02",
            new Instalacion(
                TipoInstalacion.Aclimatacion,
                "AAV02",
                5000,
                Medio.Aereo,
                TipoAlimentacion.Depredador,
                2,
                500,
                50,
                TipoRecinto.CupulaBlindada
            )
        },

        {
            "ASU04",
            new Instalacion(
                TipoInstalacion.Aclimatacion,
                "ASU04",
                3500,
                Medio.Subterraneo,
                TipoAlimentacion.Depredador,
                4,
                100,
                100,
                TipoRecinto.PozoReforzado
            )
        },

        // ---------------- Exhibición ----------------

        {
            "EDR02",
            new Instalacion(
                TipoInstalacion.Exhibicion,
                "EDR02",
                21000,
                Medio.Desierto,
                TipoAlimentacion.Recolector,
                2,
                25,
                0,
                TipoRecinto.RocaSellada
            )
        },

        {
            "EDP03",
            new Instalacion(
                TipoInstalacion.Exhibicion,
                "EDP03",
                12500,
                Medio.Desierto,
                TipoAlimentacion.Depredador,
                3,
                300,
                0,
                TipoRecinto.EscudoEstático
            )
        },

        {
            "EAV02",
            new Instalacion(
                TipoInstalacion.Exhibicion,
                "EAV02",
                15000,
                Medio.Aereo,
                TipoAlimentacion.Depredador,
                2,
                200,
                0,
                TipoRecinto.CupulaBlindada
            )
        },

        {
            "ESU03",
            new Instalacion(
                TipoInstalacion.Exhibicion,
                "ESU03",
                25000,
                Medio.Subterraneo,
                TipoAlimentacion.Depredador,
                3,
                400,
                0,
                TipoRecinto.PozoReforzado
            )
        }
    };
    }

}