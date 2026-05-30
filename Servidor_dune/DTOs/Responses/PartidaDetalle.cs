using DomainModels.Entidades;
using DomainModels.Enums;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Dune.API.DTOs.Responses
{
    public class PartidaDetalleDTO : OperacionResultado
    {
        public Guid Id { get; set; }
        public string NombreJugador { get; set; }
        public EscenarioJuego Escenario { get; set; }
        public decimal Solaris { get; set; }
        public int RondaActual { get; set; }
        public EstadoPartida EstadoPartida { get; set; }
        public int MesActual { get; set; }
        public List<EnclaveDTO> Enclaves { get; set; } = new List<EnclaveDTO>();
        public List<RegistroEventoDTO> HistorialEventos { get; set; } = new List<RegistroEventoDTO>();

        public PartidaDetalleDTO() { }

        public PartidaDetalleDTO(Partida partida)
        {
            Id = partida.Id;
            NombreJugador = partida.NombreJugador;
            Escenario = partida.Escenario;
            Solaris = partida.Solaris;
            RondaActual = partida.RondaActual;
            EstadoPartida = partida.EstadoPartida;
            MesActual = partida.MesActual;
            Enclaves = partida.Enclaves.Select(e => new EnclaveDTO(e)).ToList();
            HistorialEventos = partida.HistorialEvento.Select(he => new RegistroEventoDTO(he)).ToList();
        }
    }

    public class EnclaveDTO
    {
        public Guid Id { get; set; }
        public string Nombre { get; set; }
        public EnclaveTipo TipoEnclave { get; set; }
        public int HectareasTotales { get; set; }
        public decimal SuministrosDisponibles { get; set; }
        public int VisitantesActuales { get; set; }
        public int? NivelAdquisitivo { get; set; }
        public List<InstalacionDTO> Instalaciones { get; set; } = new List<InstalacionDTO>();

        public EnclaveDTO() { }

        public EnclaveDTO(Enclave enclave)
        {
            Id = enclave.Id;
            Nombre = enclave.Nombre;
            TipoEnclave = enclave.TipoEnclave;
            HectareasTotales = enclave.HectareasTotales;
            SuministrosDisponibles = enclave.SuministrosDisponibles;
            VisitantesActuales = enclave.VisitantesActuales;
            NivelAdquisitivo = enclave.NivelAdquisitivo;
            Instalaciones = enclave.Instalaciones.Select(i => new InstalacionDTO(i)).ToList();
        }
    }

    public class InstalacionDTO
    {
        public Guid Id { get; set; }
        public string Codigo { get; set; }
        public TipoInstalacion TipoInstalacion { get; set; }
        public int CapacidadMaxima { get; set; }
        public int CriaturasActuales { get; set; }
        public decimal SuministrosActuales { get; set; }
        public Medio MedioCompatible { get; set; }
        public TipoAlimentacion AlimentacionCompatible { get; set; }
        public List<CriaturaDTO> Criaturas { get; set; } = new List<CriaturaDTO>();

        public InstalacionDTO() { }

        public InstalacionDTO(Instalacion instalacion)
        {
            Id = instalacion.Id;
            Codigo = instalacion.Codigo;
            TipoInstalacion = instalacion.TipoInstalacion;
            CapacidadMaxima = instalacion.CapacidadMaxima;
            CriaturasActuales = instalacion.Criaturas.Count; // Asumiendo que CriaturasActuales es el conteo
            SuministrosActuales = instalacion.SuministrosActuales;
            MedioCompatible = instalacion.MedioCompatible;
            AlimentacionCompatible = instalacion.AlimentacionCompatible;
            Criaturas = instalacion.Criaturas.Select(c => new CriaturaDTO(c)).ToList();
        }
    }

    public class CriaturaDTO
    {
        public Guid Id { get; set; }
        public Especie Especie { get; set; }
        public Medio Medio { get; set; }
        public TipoAlimentacion Alimentacion { get; set; }
        public int Edad { get; set; }
        public int Salud { get; set; }
        public int VecesFavorita { get; set; }

        public CriaturaDTO() { }

        public CriaturaDTO(Criatura criatura)
        {
            Id = criatura.Id;
            Especie = criatura.Especie;
            Medio = criatura.Medio;
            Alimentacion = criatura.Alimentacion;
            Edad = criatura.Edad;
            Salud = criatura.Salud;
            VecesFavorita = criatura.VecesFavorita;
        }
    }

    public class RegistroEventoDTO
    {
        public Guid Id { get; set; }
        public DateTime FechaHora { get; set; }
        public TipoEvento TipoEvento { get; set; }
        public string Descripcion { get; set; }
        public Severidad Severidad { get; set; }

        public RegistroEventoDTO() { }

        public RegistroEventoDTO(RegistroEvento evento)
        {
            Id = evento.Id;
            FechaHora = evento.FechaHora;
            TipoEvento = evento.TipoEvento;
            Descripcion = evento.Descripcion;
            Severidad = evento.Severidad;
        }
    }
}