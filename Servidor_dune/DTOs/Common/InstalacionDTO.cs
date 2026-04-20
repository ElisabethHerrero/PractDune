namespace DTOs.Common
{
    public class InstalacionDto
    {
        public Guid Id { get; set; }
        public string Codigo { get; set; }

        public TipoInstalacion TipoInstalacion { get; set; }

        public int CapacidadMaxima { get; set; }
        public int CriaturasActuales { get; set; }

        public int SuministrosActuales { get; set; }

        public MedioCriatura MedioCompatible { get; set; }
        public TipoAlimentacion AlimentacionCompatible { get; set; }
    }
}
