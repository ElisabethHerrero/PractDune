using System;
namespace DomainModels.Catalogs.Escenario
{

    public class Escenario
    {
        public string Nombre { get; private set; }
        public string Descripcion { get; private set; }
        public int SolarisIniciales { get; private set; }
        public string EnclaveExhibicionInicial { get; private set; }

        public Escenario(string nombre, string descripcion, int solarisIniciales, string enclaveExhibicionInicial)
        {
            Nombre = nombre;
            Descripcion = descripcion;
            SolarisIniciales = solarisIniciales;
            EnclaveExhibicionInicial = enclaveExhibicionInicial;
        }
    }

}