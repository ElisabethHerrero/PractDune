using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using Newtonsoft.Json;

public class APICliente : MonoBehaviour
{
    private const string BASE_URL = " http://localhost:5079"; 

    public static APICliente Instance { get; private set; }

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public IEnumerator CrearPartida(string nombreJugador, EscenarioJuego escenario, Action<PartidaResumenDTO> onSuccess, Action<string> onError)
    {
        var requestData = new CrearPartidaRequest { NombreJugador = nombreJugador, Escenario = escenario };
        string json = JsonConvert.SerializeObject(requestData);

        using (UnityWebRequest webRequest = new UnityWebRequest(BASE_URL + "/crear", "POST"))
        {
            byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(json);
            webRequest.uploadHandler = new UploadHandlerRaw(bodyRaw);
            webRequest.downloadHandler = new DownloadHandlerBuffer();
            webRequest.SetRequestHeader("Content-Type", "application/json");

            yield return webRequest.SendWebRequest();

            if (webRequest.result == UnityWebRequest.Result.ConnectionError || webRequest.result == UnityWebRequest.Result.ProtocolError)
            {
                Debug.LogError($"Error al crear partida: {webRequest.error} - {webRequest.downloadHandler.text}");
                onError?.Invoke(webRequest.downloadHandler.text);
            }
            else
            {
                Debug.Log($"Partida creada: {webRequest.downloadHandler.text}");
                PartidaResumenDTO response = JsonConvert.DeserializeObject<PartidaResumenDTO>(webRequest.downloadHandler.text);
                onSuccess?.Invoke(response);
            }
        }
    }

    public IEnumerator ListarPartidas(Action<ListaPartidasResponse> onSuccess, Action<string> onError)
    {
        using (UnityWebRequest webRequest = UnityWebRequest.Get(BASE_URL + "/listar"))
        {
            yield return webRequest.SendWebRequest();

            if (webRequest.result == UnityWebRequest.Result.ConnectionError || webRequest.result == UnityWebRequest.Result.ProtocolError)
            {
                Debug.LogError($"Error al listar partidas: {webRequest.error} - {webRequest.downloadHandler.text}");
                onError?.Invoke(webRequest.downloadHandler.text);
            }
            else
            {
                Debug.Log($"Partidas listadas: {webRequest.downloadHandler.text}");
                ListaPartidasResponse response = JsonConvert.DeserializeObject<ListaPartidasResponse>(webRequest.downloadHandler.text);
                onSuccess?.Invoke(response);
            }
        }
    }

    public IEnumerator ObtenerDetallePartida(string partidaId, Action<PartidaDetalleDTO> onSuccess, Action<string> onError)
    {
        using (UnityWebRequest webRequest = UnityWebRequest.Get(BASE_URL + $"/{partidaId}"))
        {
            yield return webRequest.SendWebRequest();

            if (webRequest.result == UnityWebRequest.Result.ConnectionError || webRequest.result == UnityWebRequest.Result.ProtocolError)
            {
                Debug.LogError($"Error al obtener detalle de partida: {webRequest.error} - {webRequest.downloadHandler.text}");
                onError?.Invoke(webRequest.downloadHandler.text);
            }
            else
            {
                Debug.Log($"Detalle de partida: {webRequest.downloadHandler.text}");
                PartidaDetalleDTO response = JsonConvert.DeserializeObject<PartidaDetalleDTO>(webRequest.downloadHandler.text);
                onSuccess?.Invoke(response);
            }
        }
    }

    public IEnumerator EjecutarRonda(string partidaId, Action<PartidaDetalleDTO> onSuccess, Action<string> onError)
    {
        using (UnityWebRequest webRequest = new UnityWebRequest(BASE_URL + $"/ejecutarRonda/{partidaId}", "POST"))
        {
            webRequest.downloadHandler = new DownloadHandlerBuffer();
            webRequest.SetRequestHeader("Content-Type", "application/json");

            yield return webRequest.SendWebRequest();

            if (webRequest.result == UnityWebRequest.Result.ConnectionError || webRequest.result == UnityWebRequest.Result.ProtocolError)
            {
                Debug.LogError($"Error al ejecutar ronda: {webRequest.error} - {webRequest.downloadHandler.text}");
                onError?.Invoke(webRequest.downloadHandler.text);
            }
            else
            {
                Debug.Log($"Ronda ejecutada: {webRequest.downloadHandler.text}");
                PartidaDetalleDTO response = JsonConvert.DeserializeObject<PartidaDetalleDTO>(webRequest.downloadHandler.text);
                onSuccess?.Invoke(response);
            }
        }
    }

    // ... Otros métodos para GuardarPartida, EliminarPartida, etc.
}
