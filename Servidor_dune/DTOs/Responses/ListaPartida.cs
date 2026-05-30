using System.Collections.Generic;

namespace Dune.API.DTOs.Responses
{
    public class ListaPartidasResponse : OperacionResultado
    {
        public List<PartidaResumenDTO> Partidas { get; set; } = new List<PartidaResumenDTO>();
        public int Cantidad => Partidas.Count;
    }
}