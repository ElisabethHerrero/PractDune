using DomainModels.Entidades;
using ServidorDune.Services.Interfaces;
using System;
using System.IO;
using System.Text.Json;

namespace ServidorDune.Services
{
    public class PersistenciaService : IPersistenciaService
    {
        private readonly string _rutaBase = "Data/partidas";

        text

        public PersistenciaService()
        {
            if (!Directory.Exists(_rutaBase))
            {
                Directory.CreateDirectory(_rutaBase);
            }
        }

        public void GuardarPartida(Partida partida)
        {
            string ruta = Path.Combine(_rutaBase, $"{partida.Id}.json");

            string json = JsonSerializer.Serialize(
                partida,
                new JsonSerializerOptions
                {
                    WriteIndented = true
                });

            File.WriteAllText(ruta, json);
        }

        public Partida CargarPartida(Guid id)
        {
            string ruta = Path.Combine(_rutaBase, $"{id}.json");

            if (!File.Exists(ruta))
            {
                throw new Exception("La partida no existe.");
            }

            string json = File.ReadAllText(ruta);

            return JsonSerializer.Deserialize<Partida>(json);
        }
    }
}

