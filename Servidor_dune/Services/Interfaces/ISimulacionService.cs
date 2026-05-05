using DomainModels.Entidades;
using DomainModels.Enums;

namespace ServidorDune.Services.Interfaces
{
    public interface IRegistroEventosService
    {
        void RegistrarEvento(
        Partida partida,
        TipoEvento tipoEvento,
        string descripcion,
        SeveridadEvento severidad);
    }
}