using System;
using DomainModels.Catalogs.Especie;
using DomainModels.Enums;

namespace DomainModels.Entidades
{
    public class Criatura
    {
        public Guid Id { get; private set; }

        public TipoEspecie Especie { get; private set; }
        public Medio Medio { get; private set; }
        public TipoAlimentacion Alimentacion { get; private set; }

        public int Edad { get; private set; }
        public int EdadAdulta { get; private set; }
        public int Salud { get; private set; } = 100;
        public int VecesFavorita { get; private set; }

        private int ApetitoBase;

        public EstadoCriatura EstadoCriatura { get; set; } = EstadoCriatura.Activa;


        public Criatura(TipoEspecie especie)
        {
            Id = Guid.NewGuid();
            Especie = especie;

            // Aquí se obtienen los datos sin necesidad de un switch interno
            var datos = EspecieConfig.Obtener(especie);

            Medio = datos.Medio;
            Alimentacion = datos.Alimentacion;
            EdadAdulta = datos.EdadAdulta;
            ApetitoBase = datos.ApetitoBase;

            Edad = 0;
            Salud = 100;
            VecesFavorita = 0;

            EstadoCriatura = EstadoCriatura.Activa;


        }

        public int CalcularIngesta(bool esAclimatacion)
        {
            if (Edad < EdadAdulta)
                return ApetitoBase * Edad;

            int alpha = esAclimatacion ? 15 : 1;
            return (int)(ApetitoBase * Math.Pow(2, Edad - EdadAdulta) * alpha);
        }

        public void Alimentar(int cantidad, bool esAclimatacion)
        {
            if (EstadoCriatura != EstadoCriatura.Activa)
                return;

            int necesaria = CalcularIngesta(esAclimatacion);

            if (necesaria <= 0)
                return;

            double porcentaje = (double)cantidad / necesaria;

            if (porcentaje < 0.25) Salud -= 30;
            else if (porcentaje < 0.75) Salud -= 20;
            else if (porcentaje < 1) Salud -= 10;
            else if (Salud < 100) Salud += 5;

            if (Salud < 0) Salud = 0;

            if (Salud <= 0)
            {
                Salud = 0;
                EstadoCriatura = EstadoCriatura.LetargoIrreversible;
            }

            if (Salud > 100)
                Salud = 100;
        

        }

    public void Envejecer()
        {
            if (Salud <= 0)
            {
                Salud = 0;
                EstadoCriatura = EstadoCriatura.LetargoIrreversible;
            }

            if (Salud > 100)
                Salud = 100;
        }

    

    public bool EsAdulta() => Edad >= EdadAdulta;

        public void MarcarFavorita()
        {
            VecesFavorita++;
        }

    }

}