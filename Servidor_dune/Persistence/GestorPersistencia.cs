using System;
using System.IO;
using System.Text.Json;
using System.Collections.Generic;
using DomainModels;


namespace Persistence
{
    public class GestorPersistencia
    {
        private readonly string _dataDirectory;

        public GestorPersistencia(string dataDirectory)
        {
            _dataDirectory = dataDirectory;
            if (!Directory.Exists(_dataDirectory))
            {
                Directory.CreateDirectory(_dataDirectory);
            }
        }

        public void GuardarPartida(Partida partida)
        {
            if (partida == null)
            {
                throw new ArgumentNullException(nameof(partida));
            }

            // Usamos el ID de la partida para el nombre del archivo, asegurando unicidad.
            // Podríamos añadir el alias del jugador para facilitar la identificación visual.
            string fileName = $"Partida_{partida.Id}.json";
            string filePath = Path.Combine(_dataDirectory, fileName);

            var options = new JsonSerializerOptions { WriteIndented = true };
            string jsonString = JsonSerializer.Serialize(partida, options);
            File.WriteAllText(filePath, jsonString);
            Console.WriteLine($"Partida '{partida.AliasJugador}' guardada en: {filePath}");
        }

        public Partida CargarPartida(Guid partidaId)
        {
            string fileName = $"Partida_{partidaId}.json";
            string filePath = Path.Combine(_dataDirectory, fileName);

            if (!File.Exists(filePath))
            {
                throw new FileNotFoundException($"No se encontró la partida con ID {partidaId} en {filePath}");
            }

            string jsonString = File.ReadAllText(filePath);
            Partida? partida = JsonSerializer.Deserialize<Partida>(jsonString);

            if (partida == null)
            {
                throw new InvalidOperationException($"No se pudo deserializar la partida desde {filePath}");
            }
            Console.WriteLine($"Partida '{partida.AliasJugador}' cargada desde: {filePath}");
            return partida;
        }

        public List<Guid> ListarPartidasGuardadas()
        {
            List<Guid> partidas = new List<Guid>();
            foreach (string filePath in Directory.GetFiles(_dataDirectory, "Partida_*.json"))
            {
                string fileName = Path.GetFileNameWithoutExtension(filePath);
                string idString = fileName.Replace("Partida_", "");
                if (Guid.TryParse(idString, out Guid partidaId))
                {
                    partidas.Add(partidaId);
                }
            }
            return partidas;
        }
    }
}