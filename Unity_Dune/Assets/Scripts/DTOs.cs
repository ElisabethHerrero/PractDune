using Newtonsoft.Json.Converters;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

// DTOs para Unity (deben coincidir con los del backend)

// Atributo esencial para que Unity pueda serializar/deserializar estos objetos
[System.Serializable]
public class CrearPartidaRequest
{
    public string Nombre;
    // El enum EscenarioJuego del backend se envía como string
    public EscenarioJuego Escenario;
}

[System.Serializable]
public class OperacionResultado
{
    public bool Success;
    public string Message;
}

[System.Serializable]
public class PartidaResumenDTO : OperacionResultado
{
    // Guid del backend se recibe como string
    public Guid Id;
    public string NombreJugador;
    public string Escenario; // String para el enum
    public double Solaris; // decimal del backend se recibe como float o double
    public int RondaActual;
    public string EstadoPartida; // String para el enum
}

[System.Serializable]
public class ListaPartidasResponse : OperacionResultado
{
    public List<PartidaResumenDTO> Partidas = new List<PartidaResumenDTO>();
    public int Cantidad;
}

[System.Serializable]
public class PartidaDetalleDTO : OperacionResultado
{
    public Guid Id;
    public string NombreJugador;
    public string Escenario;
    public float Solaris;
    public int RondaActual;
    public string EstadoPartida;
    //?
    public List<EnclaveDTO> Enclaves = new List<EnclaveDTO>();
    public List<RegistroEventoDTO> HistorialEventos = new List<RegistroEventoDTO>();
}

[System.Serializable]
public class EnclaveDTO
{
    public Guid Id;
    public string Nombre;
    public TipoEnclave TipoEnclave; // String para el enum
    public double HectareasTotales;
    public int SuministrosDisponibles; // decimal
    public int VisitantesActuales;
    public int? NivelAdquisitivo; // int? se mantiene como int? o int si siempre tiene valor
    public List<InstalacionDTO> Instalaciones = new List<InstalacionDTO>();
}

[System.Serializable]
public class InstalacionDTO
{
    public Guid Id;
    public string Codigo;

    public string TipoInstalacion; // String para el enum

    public int CapacidadMaxima;
    public int CriaturasActuales;

    public float SuministrosActuales; // decimal

    public string MedioCompatible; // String para el enum
    public string AlimentacionCompatible; // String para el enum
    public List<CriaturaDTO> Criaturas = new List<CriaturaDTO>();
}

[System.Serializable]
public class CriaturaDTO
{
    public Guid Id { get; set; }

    // Especie lo dejamos como string por lo que hablamos antes (flexibilidad)
    public string Especie { get; set; }

    public int EdadActual { get; set; }
    public int Salud { get; set; }

    // Usamos [JsonConverter] para que Newtonsoft envíe el nombre ("Aereo") 
    // en lugar del número (2). ¡Esto es clave para Unity!
    [JsonConverter(typeof(StringEnumConverter))]
    public Medio Medio { get; set; }

    [JsonConverter(typeof(StringEnumConverter))]
    public TipoAlimentacion TipoAlimentacion { get; set; }

    public int VecesFavorita { get; set; }

    [JsonConverter(typeof(StringEnumConverter))]
    public EstadoCriatura EstadoCriatura { get; set; }
}

[System.Serializable]
public class RegistroEventoDTO
{
    public string Id;
    public string FechaHora; // DateTime se recibe como string
    public TipoEvento TipoEvento; // String para el enum
    public string Descripcion;
    public Severidad Severidad; // String para el enum
}

public class EnclaveDto
{
    public DateTime FechaHora { get; set; }
    public TipoEvento TipoEvento { get; set; }
    public string Descripcion { get; set; }
    public Severidad Severidad { get; set; }
}


//Catalogs
public class Especie
{
    public Medio Medio;
    public TipoAlimentacion Alimentacion ;
    public int EdadAdulta;
    public int ApetitoBase;
}


//enums

public enum EscenarioJuego
{
    Arrakeen,
    GiediPrime,
    Caladan
}


public enum TipoEnclave
{
    Aclimatacion,
    Exhibicion
}

public enum Medio
{
    Desierto,
    Subterraneo,
    Aereo
}

public enum TipoAlimentacion
{
    Recolector,
    Depredador
}

public enum EstadoCriatura
{
    Activa,
    LetargoIrreversible,
    Trasladada,
    Descartada
}

public enum TipoEvento
{
    CreacionPartida,
    ConstruccionInstalacion,
    Alimentacion,
    Reproduccion,
    Traslado,
    Descarte,
    SimulacionRonda,
    GuardadoPartida,
    ErrorComunicacion
}

public enum Severidad
{
    Info,
    Warning,
    Error
}



