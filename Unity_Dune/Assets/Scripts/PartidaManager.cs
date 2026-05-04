using System.Collections;
using System.Diagnostics;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;

public class PartidaManager : MonoBehaviour
{
    public void CrearPartida(string nombre, int tipoEscenario)
    {
        StartCoroutine(EnviarPeticionCrearPartida(nombre, tipoEscenario));
    }

    private IEnumerator EnviarPeticionCrearPartida(string nombre, int tipoEscenario)
    {
        string url = "https://localhost:5001/api/partida/crear"; // URL de tu API

        // Crear el JSON con los datos
        string jsonBody = $"{{\"NombreJugador\":\"{nombre}\", \"TipoEscenario\":{tipoEscenario}}}";

        using (UnityWebRequest request = new UnityWebRequest(url, "POST"))
        {
            byte[] bodyRaw = Encoding.UTF8.GetBytes(jsonBody);
            request.uploadHandler = new UploadHandlerRaw(bodyRaw);
            request.downloadHandler = new DownloadHandlerBuffer();
            request.SetRequestHeader("Content-Type", "application/json");

            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.Success)
            {
                Debug.Log("Partida creada: " + request.downloadHandler.text);
                // Aquí puedes procesar la respuesta y cambiar de escena
            }
            else
            {
                Debug.LogError("Error al crear partida: " + request.error);
            }
        }
    }
}
