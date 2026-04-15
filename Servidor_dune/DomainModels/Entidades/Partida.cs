namespace Entities;

public class Enclave
{
    public int Id { get; set; }
    public string Nombre { get; set; } = string.Empty;

    public TipoEnclave TipoEnclave { get; set; }

    public double HectareasTotales { get; set; }

    public int SuministrosDisponibles { get; set; }

    public int VisitantesMensualesBase { get; set; }

    public int VisitantesActuales { get; set; }

    public decimal NivelAdquisitivo { get; set; }

    public List<Instalacion> Instalaciones { get; set; } = new();
}