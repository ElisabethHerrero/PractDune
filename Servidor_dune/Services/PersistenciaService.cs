using DomainModels.Entidades;
using ServidorDune.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;

namespace ServidorDune.Services
{
    public class PersistenciaService : IPersistenciaService
    {
        private readonly string _rutaBase = Path.Combine("Data", "Partidas");

        private readonly JsonSerializerOptions _opcionesJson = new JsonSerializerOptions
        {
            WriteIndented = true,
            PropertyNameCaseInsensitive = true
        };

        public PersistenciaService()
        {
            Directory.CreateDirectory(_rutaBase);
        }

        public void GuardarPartida(Partida partida)
        {
            if (partida == null)
                throw new ArgumentNullException(nameof(partida));

            string ruta = Path.Combine(_rutaBase, $"{partida.Id}.json");
            string json = JsonSerializer.Serialize(partida, _opcionesJson);

            File.WriteAllText(ruta, json);
        }

        public Partida CargarPartida(Guid idPartida)
        {
            string ruta = Path.Combine(_rutaBase, $"{idPartida}.json");

            if (!File.Exists(ruta))
                throw new FileNotFoundException("La partida no existe.");

            string json = File.ReadAllText(ruta);

            Partida? partida = JsonSerializer.Deserialize<Partida>(json, _opcionesJson);

            if (partida == null)
                throw new InvalidOperationException("No se pudo deserializar la partida.");

            return partida;
        }

        public List<Partida> CargarTodasLasPartidas()
        {
            List<Partida> partidas = new();

            foreach (string archivo in Directory.GetFiles(_rutaBase, "*.json"))
            {
                string json = File.ReadAllText(archivo);
                Partida? partida = JsonSerializer.Deserialize<Partida>(json, _opcionesJson);

                if (partida != null)
                    partidas.Add(partida);
            }

            return partidas;
        }
    }
}