namespace DTOs.Common
{
    public class EnclaveDto
    {
        public Guid Id { get; set; }
        public string Nombre { get; set; }
        public TipoEnclave TipoEnclave { get; set; }

        public double HectareasTotales { get; set; }
        public int SuministrosDisponibles { get; set; }
        public int VisitantesActuales { get; set; }

        public NivelAdquisitivo NivelAdquisitivo { get; set; }

        public List<InstalacionDto> Instalaciones { get; set; } = new();
    }
}
