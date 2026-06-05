using DomainModels.Catalogs.Especie;
using DomainModels.Entidades;
using DomainModels.Enums;
using ServidorDune.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;


namespace ServidorDune.Services
{
    public class SimulacionService : ISimulacionService
    {
        private readonly IRegistroEventosService _registroEventosService;
        private readonly Random _random = new Random();


        public SimulacionService(IRegistroEventosService registroEventosService)
        {
            _registroEventosService = registroEventosService;
        }

        public void EjecutarRonda(Partida partida)
        {
            if (partida == null)
                throw new ArgumentNullException(nameof(partida));

            if (partida.EstadoPartida == EstadoPartida.Finalizada)
                throw new InvalidOperationException("No se puede ejecutar una ronda en una partida finalizada.");

            foreach (Enclave enclave in partida.Enclaves)
            {
                //bool esAclimatacion = enclave.TipoEnclave == TipoEnclave.Aclimatacion;

                foreach (Instalacion instalacion in enclave.Instalaciones)
                {
                    instalacion.AlimentarCriaturas(true); //esAclimatación iba aqui

                    EnvejecerCriaturas(instalacion);
                    if (instalacion.tipoInstalacion == TipoInstalacion.Aclimatacion)
                    {
                        IntentarGenerarCriatura(partida, enclave, instalacion);
                    }


                }
            }

            


            partida.RondaActual++;

            if (partida.EstadoPartida == EstadoPartida.Creada)
                partida.EstadoPartida = EstadoPartida.EnCurso;

            _registroEventosService.RegistrarEvento(
                partida,
                TipoEvento.SimulacionRonda,
                $"Se ejecutó la ronda {partida.RondaActual}.",
                Severidad.Info);
        }

        private void EnvejecerCriaturas(Instalacion instalacion)
        {
            foreach (Criatura criatura in instalacion.Criaturas)
            {
                criatura.Envejecer();
            }
        }

        private void IntentarGenerarCriatura(
            Partida partida,
            Enclave enclave,
            Instalacion instalacion)
        {
            if (!instalacion.TieneCapacidad())
                return;

            int probabilidad = _random.Next(0, 100);

            if (probabilidad >= 80)
                return;

            List<TipoEspecie> especiesCompatibles =
                ObtenerEspeciesCompatibles(instalacion);

            //if (especiesCompatibles.Count == 0)
            //    return;

            TipoEspecie especieElegida =
                especiesCompatibles[_random.Next(especiesCompatibles.Count)];

            Criatura nuevaCriatura = new Criatura(especieElegida);

            instalacion.AñadirCriatura(nuevaCriatura);

            _registroEventosService.RegistrarEvento(
                partida,
                TipoEvento.Reproduccion,
                $"Nueva criatura generada: {nuevaCriatura.Especie} en instalación {instalacion.Codigo} del enclave {enclave.Nombre}.",
                Severidad.Info);
        }

        private List<TipoEspecie> ObtenerEspeciesCompatibles(Instalacion instalacion)
        {
            List<TipoEspecie> compatibles = new List<TipoEspecie>();

            foreach (TipoEspecie especie in Enum.GetValues(typeof(TipoEspecie)))
            {
                Especie datos = EspecieConfig.Obtener(especie);

                if (datos.Medio == instalacion.Medio &&
                    datos.Alimentacion == instalacion.Alimentacion)
                {
                    compatibles.Add(especie);
                }
            }

            return compatibles;
        }



    }
}