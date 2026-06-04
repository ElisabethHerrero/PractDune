using DomainModels.Enums;
using System;
using System.Collections.Generic;

namespace DomainModels.Entidades
{

    public class Enclave
    {
        public Guid Id { get; set; }
        public string Nombre { get; set; }
        public EscenarioJuego TipoEnclave { get; set; }

        public int HectareasTotales { get; set; }
        public int SuministrosDisponibles { get; set; }

        public int VisitantesMensualesBase { get; set; }
        public int VisitantesActuales { get; set; }
        public NivelAdquisitivo NivelAdquisitivo { get; set; }

        public List<Instalacion> Instalaciones { get; set; } = new();

        public Enclave( string nombre, EscenarioJuego tipoEnclave, int hectareasTotales, int visitantesMensualesBase, NivelAdquisitivo nivelAdquisitivo)
        {
            Id = Guid.NewGuid();
            Nombre = nombre;
            TipoEnclave = tipoEnclave;

            HectareasTotales = hectareasTotales;
            VisitantesMensualesBase = visitantesMensualesBase;
            VisitantesActuales = visitantesMensualesBase;

            NivelAdquisitivo = nivelAdquisitivo;

            SuministrosDisponibles = 0;
        }
    }

}
