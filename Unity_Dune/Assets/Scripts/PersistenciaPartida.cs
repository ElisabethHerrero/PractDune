using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PersistenciaPartida : MonoBehaviour
{
    // URL de la API (cambiar según tu servidor)
    private string apiUrl = "http://localhost:5000/api/partida";

    /// <summary>
    /// Crea una nueva partida llamando a la API
    /// </summary>
    public void CrearPartida(string nombreJugador, int escenario)
    {
        StartCoroutine(CrearPartidaCoroutine(nombreJugador, escenario));
    }

    private IEnumerator CrearPartidaCoroutine(string nombreJugador, int escenario)
    {
        // Preparar los datos a enviar
        var requestData = new
        {
            nombreJugador = nombreJugador,
            escenario = escenario // 0 = Arrakeen, 1 = Giedi Prime, 2 = Caladan
        };

        string json = JsonUtility.ToJson(requestData);
        byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(json);

        // Crear el request
        using (UnityWebRequest request = new UnityWebRequest(apiUrl + "/crear", "POST"))
        {
            request.uploadHandler = new UploadHandlerRaw(bodyRaw);
            request.downloadHandler = new DownloadHandlerBuffer();
            request.SetRequestHeader("Content-Type", "application/json");

            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.Success)
            {
                Debug.Log("Partida creada: " + request.downloadHandler.text);
                // Aquí puedes procesar la respuesta y obtener el ID de la partida
            }
            else
            {
                Debug.LogError("Error al crear partida: " + request.error);
            }
        }
    }

    /// <summary>
    /// Carga una partida existente desde la API
    /// </summary>
    public void CargarPartida(string partidaId)
    {
        StartCoroutine(CargarPartidaCoroutine(partidaId));
    }

    private IEnumerator CargarPartidaCoroutine(string partidaId)
    {
        using (UnityWebRequest request = UnityWebRequest.Get(apiUrl + "/cargar/" + partidaId))
        {
            request.SetRequestHeader("Content-Type", "application/json");
            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.Success)
            {
                Debug.Log("Partida cargada: " + request.downloadHandler.text);
                // Aquí procesas los datos de la partida
            }
            else
            {
                Debug.LogError("Error al cargar partida: " + request.error);
            }
        }
    }

    /// <summary>
    /// Guarda una partida (actualización)
    /// </summary>
    public void GuardarPartida(string partidaJson)
    {
        StartCoroutine(GuardarPartidaCoroutine(partidaJson));
    }

    private IEnumerator GuardarPartidaCoroutine(string partidaJson)
    {
        byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(partidaJson);

        using (UnityWebRequest request = new UnityWebRequest(apiUrl + "/guardar", "POST"))
        {
            request.uploadHandler = new UploadHandlerRaw(bodyRaw);
            request.downloadHandler = new DownloadHandlerBuffer();
            request.SetRequestHeader("Content-Type", "application/json");

            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.Success)
            {
                Debug.Log("Partida guardada: " + request.downloadHandler.text);
            }
            else
            {
                Debug.LogError("Error al guardar partida: " + request.error);
            }
        }
    }

    /// <summary>
    /// Lista todas las partidas guardadas
    /// </summary>
    public void ListarPartidas()
    {
        StartCoroutine(ListarPartidasCoroutine());
    }

    private IEnumerator ListarPartidasCoroutine()
    {
        using (UnityWebRequest request = UnityWebRequest.Get(apiUrl + "/listar"))
        {
            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.Success)
            {
                Debug.Log("Partidas disponibles: " + request.downloadHandler.text);
            }
            else
            {
                Debug.LogError("Error al listar partidas: " + request.error);
            }
        }
    }

    /// <summary>
    /// Elimina una partida
    /// </summary>
    public void EliminarPartida(string partidaId)
    {
        StartCoroutine(EliminarPartidaCoroutine(partidaId));
    }

    private IEnumerator EliminarPartidaCoroutine(string partidaId)
    {
        using (UnityWebRequest request = UnityWebRequest.Delete(apiUrl + "/eliminar/" + partidaId))
        {
            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.Success)
            {
                Debug.Log("Partida eliminada: " + request.downloadHandler.text);
            }
            else
            {
                Debug.LogError("Error al eliminar partida: " + request.error);
            }
        }
    }
}

