using System;
using System.Collections.Generic;
using DomainModels.Enum;

namespace DomainModels.Entidades
{

    public class Instalacion
    {
        public TipoInstalacion tipoInstalacion { get; private set; }
        public string Codigo { get; private set; }
        public int Coste { get; private set; }
        public Medio Medio { get; private set; }
        public TipoAlimentacion Alimentacion { get; private set; }
        public int Capacidad { get; private set; }
        public int Hectareas { get; private set; }
        public int Suministros { get; private set; }
        public TipoRecinto TipoRecinto { get; private set; }

        public List<Criatura> Criaturas { get; private set; } = new();

        public bool TieneCapacidad() => Criaturas.Count < Capacidad;

        public void AñadirCriatura(Criatura c)
        {
            if (!TieneCapacidad())
                throw new Exception("Instalación llena");

            if (c.Medio != Medio || c.Alimentacion != Alimentacion)
                throw new Exception("Criatura incompatible");

            Criaturas.Add(c);
        }

        public void AlimentarCriaturas(bool esAclimatacion)
        {
            foreach (var c in Criaturas)
            {
                int necesaria = c.CalcularIngesta(esAclimatacion);

                if (Suministros >= necesaria)
                {
                    Suministros -= necesaria;
                    c.Alimentar(necesaria, esAclimatacion);
                }
                else
                {
                    c.Alimentar(Suministros, esAclimatacion);
                    Suministros = 0;
                }
            }
        }
    }

}
