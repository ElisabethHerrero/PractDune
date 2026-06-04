using System;

namespace DTOs.Request
{

    public class ConstruirInstalacionRequest
    {
        public Guid PartidaId { get; set; }
        public Guid EnclaveId { get; set; }
        public string Codigo { get; set; }
        // Podrías añadir más propiedades si la construcción requiere otros parámetros
    }


}