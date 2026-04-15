using System;
using System.Collections.Generic;

public class Enclave
{
    public Guid Id { get; set; }
    public string Nombre { get; set; }
    public TipoEnclave TipoEnclave { get; set; }

    public double HectareasTotales { get; set; }
    public int SuministrosDisponibles { get; set; }

    public int VisitantesMensualesBase { get; set; }
    public int VisitantesActuales { get; set; }
    public int NivelAdquisitivo { get; set; }

    public List<Instalacion> Instalaciones { get; set; } = new();
}
