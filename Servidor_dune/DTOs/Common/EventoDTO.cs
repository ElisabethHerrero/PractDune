namespace DTOs.Common
{
    public class EnclaveDto
    {
        public int Id { get; set; }
        public string FechaHora { get; set; }
        public TipoEvento TipoEvento { get; set; }
        public string Descripcion { get; set; }
        public SeveridadEvento Severidad { get; set; }
    }
}