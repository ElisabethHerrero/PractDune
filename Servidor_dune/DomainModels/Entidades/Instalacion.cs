using System;
using System.Collections.Generic;

public class Instalacion
{
    public Guid Id { get; set; }
    public string Codigo { get; set; }
    public string Nombre { get; set; }

    public TipoInstalacion TipoInstalacion { get; set; }
    public decimal CosteConstruccion { get; set; }

    public Medio MedioCompatible { get; set; }
    public TipoAlimentacion AlimentacionCompatible { get; set; }

    public int SuministrosActuales { get; set; }
    public int CapacidadMaxima { get; set; }
    public double Hectareas { get; set; }

    public TipoRecinto TipoRecinto { get; set; }

    public List<Criatura> Criaturas { get; set; } = new();
}
