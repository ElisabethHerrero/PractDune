using System;
using System.Collections.Generic;
using DomainModels.Enums;

namespace DomainModels.Entidades
{
    public class Partida
    {
        public Guid Id { get; set; } //esto es mejor que un int para los id

        // Mantenemos NombreJugador porque lo estás usando en el método de inicialización
        public string NombreJugador { get; set; }

        public EscenarioJuego Escenario { get; set; }

        public decimal Solaris { get; set; }

        //public DateTime FechaCreacion { get; set; } //esto no es necesario

        // Mantenemos MesActual porque lo usas en InicializarNueva
        public int MesActual { get; set; }

        // Puedes usarlo también como contador de rondas si quieres
        public int RondaActual { get; set; }

        public EstadoPartida EstadoPartida { get; set; }

        public List<Enclave> Enclaves { get; set; } = new();

        // Mantenemos HistorialEventos porque lo usas en RegistrarEvento
        public List<RegistroEvento> HistorialEventos { get; set; } = new();

        public static Partida InicializarNueva(string nombreJugador, EscenarioJuego tipoEscenario)
        {
            // 1. Obtenemos la configuración del escenario desde nuestra clase externa
            var config = EscenarioConfig.DatosEscenarios[tipoEscenario];

            var partida = new Partida
            {
                Id = Guid.NewGuid(),

                NombreJugador = nombreJugador,

                Escenario = tipoEscenario,

                Solaris = config.SolarisIniciales,

                MesActual = 1,

                RondaActual = 1,

                EstadoPartida = EstadoPartida.Creada
            };

            // 2. Añadimos el enclave de exhibición según la config
            partida.Enclaves.Add(new Enclave
            {
                Id = Guid.NewGuid(),
                Nombre = config.NombreEnclave,
                TipoEnclave = TipoEnclave.Exhibicion,
                HectareasTotales = config.Hectareas,
                VisitantesMensualesBase = config.Visitantes,
                VisitantesActuales = config.Visitantes,
                NivelAdquisitivo = (int)config.Nivel
            });

            // 3. Añadimos el enclave de aclimatación común (Sección 3.3)
            partida.Enclaves.Add(new Enclave
            {
                Id = Guid.NewGuid(),
                Nombre = "Cuenca Experimental",
                TipoEnclave = TipoEnclave.Aclimatacion,
                HectareasTotales = 5000,
                VisitantesMensualesBase = 0,
                VisitantesActuales = 0,
                NivelAdquisitivo = 0
            });

            partida.RegistrarEvento(
                TipoEvento.CreacionPartida,
                $"Partida creada en {tipoEscenario}.",
                SeveridadEvento.Info
            );

            return partida;
        }

        public void RegistrarEvento(
            TipoEvento tipo,
            string desc,
            SeveridadEvento severidad)
        {
            HistorialEventos.Add(new RegistroEvento
            {
                Id = Guid.NewGuid(),

                FechaHora = DateTime.Now,

                TipoEvento = tipo,

                Descripcion = desc,

                Severidad = severidad,

                PartidaId = this.Id // o de donde venga
            });
        }
    }
}