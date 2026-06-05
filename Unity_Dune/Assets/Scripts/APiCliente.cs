using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using Newtonsoft.Json;
using System.Text;
using Newtonsoft;

public class APICliente : MonoBehaviour
{
    private const string BASE_URL = "http://localhost:5018/api/Partida"; 

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
        var requestData = new CrearPartidaRequest { Nombre = nombreJugador, Escenario = escenario };
        string json = JsonConvert.SerializeObject(requestData);

        using (UnityWebRequest webRequest = new UnityWebRequest(BASE_URL + "/Crear", "POST"))
        {
            byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(json);
            webRequest.uploadHandler = new UploadHandlerRaw(bodyRaw);
            webRequest.downloadHandler = new DownloadHandlerBuffer();
            webRequest.SetRequestHeader("Content-Type", "application/json");

            webRequest.certificateHandler = new BypassCertificate();

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

    public class BypassCertificate : UnityEngine.Networking.CertificateHandler
    {
        protected override bool ValidateCertificate(byte[] certificateData)
        {
            return true; // Confía en todos los certificados
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

    public IEnumerator ObtenerDetallePartida(Guid partidaId, Action<PartidaDetalleDTO> onSuccess, Action<string> onError)
    {
        // IMPORTANTE: Asegúrate de añadir "/Cargar/" en la URL
        // Si tu BASE_URL es ".../api/Partida", la ruta final debe ser ".../api/Partida/Cargar/ID"
        string urlCompleta = BASE_URL + "/" + partidaId.ToString();

        using (UnityWebRequest webRequest = UnityWebRequest.Get(urlCompleta))
        {
            yield return webRequest.SendWebRequest();

            if (webRequest.result == UnityWebRequest.Result.ConnectionError || webRequest.result == UnityWebRequest.Result.ProtocolError)
            {
                Debug.LogError($"Error al obtener detalle: {webRequest.error} - {webRequest.downloadHandler.text}");
                onError?.Invoke(webRequest.downloadHandler.text);
            }
            else
            {
                // 2. Obtenemos el texto del JSON
                string jsonRecibido = webRequest.downloadHandler.text;
                Debug.Log($"JSON recibido: {jsonRecibido}");

                try
                {
                    // 3. Deserializamos usando Newtonsoft.Json (la variable 'response' ya contiene todo)
                    PartidaDetalleDTO response = JsonConvert.DeserializeObject<PartidaDetalleDTO>(jsonRecibido);

                    // 4. Verificamos y asignamos el Enclave al GameManager ANTES del callback
                    if (response != null && response.Enclaves != null && response.Enclaves.Count > 0)
                    {
                        // Asignamos el ID del primer enclave al GameManager
                        GameManager.Instance.SelectEnclave(response.Enclaves[0].Id);
                        Debug.Log($"[GameManager] Enclave inicial asignado: {response.Enclaves[0].Nombre} ({response.Enclaves[0].Id})");
                    }
                    else
                    {
                        Debug.LogWarning("[GameManager] La partida no contiene enclaves.");
                    }

                    // 5. Notificamos el éxito al resto del juego
                    onSuccess?.Invoke(response);
                }
                catch (Exception ex)
                {
                    Debug.LogError($"Error al procesar el JSON: {ex.Message}");
                    onError?.Invoke("Error de deserialización");
                }
            }

        }
    }

    public IEnumerator EjecutarRonda(Guid partidaId, Action<PartidaDetalleDTO> onSuccess, Action<string> onError)
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

                PartidaDetalleDTO response =
                    JsonConvert.DeserializeObject<PartidaDetalleDTO>(webRequest.downloadHandler.text);

                onSuccess?.Invoke(response);
            }
        }
    }


    // ... Otros métodos para GuardarPartida, EliminarPartida, etc.

    public IEnumerator CargarPartidaCo(Guid partidaId)
    {
        using (UnityWebRequest webRequest = UnityWebRequest.Get(BASE_URL + $"/cargar/{partidaId}"))
        {
            yield return webRequest.SendWebRequest();

            if (webRequest.result == UnityWebRequest.Result.ConnectionError || webRequest.result == UnityWebRequest.Result.ProtocolError)
            {
                Debug.LogError($"Error al cargar partida {partidaId}: {webRequest.error}");
            }
            else
            {
                string jsonResponse = webRequest.downloadHandler.text;
                PartidaDetalleDTO partidaCargada = JsonConvert.DeserializeObject<PartidaDetalleDTO>(jsonResponse);
                Debug.Log($"Partida cargada exitosamente: {partidaCargada.NombreJugador} - Solaris: {partidaCargada.Solaris}");
                
            }
        }
    }

    //
    /*
    private IEnumerator ConstruirInstalacionCo(string tipoInstalacion)
    {
        if (string.IsNullOrEmpty(currentPartidaId) || string.IsNullOrEmpty(currentEnclaveId))
        {
            Debug.LogError("Partida o Enclave no seleccionados. No se puede construir.");
            yield break;
        }

        ConstruirInstalacionRequestUnity requestData = new ConstruirInstalacionRequestUnity
        {
            PartidaId = currentPartidaId,
            EnclaveId = currentEnclaveId,
            TipoInstalacion = tipoInstalacion
        };

        string jsonRequestBody = JsonConvert.SerializeObject(requestData);

        using (UnityWebRequest webRequest = new UnityWebRequest(API_BASE_URL + "/construir-instalacion", "POST"))
        {
            byte[] bodyRaw = Encoding.UTF8.GetBytes(jsonRequestBody);
            webRequest.uploadHandler = new UploadHandlerRaw(bodyRaw);
            webRequest.downloadHandler = new DownloadHandlerBuffer();
            webRequest.SetRequestHeader("Content-Type", "application/json");

            yield return webRequest.SendWebRequest();

            if (webRequest.result == UnityWebRequest.Result.ConnectionError || webRequest.result == UnityWebRequest.Result.ProtocolError)
            {
                Debug.LogError($"Error al construir instalación: {webRequest.error}");
            }
            else
            {
                string jsonResponse = webRequest.downloadHandler.text;
                ConstruirInstalacionResponse response = JsonConvert.DeserializeObject<ConstruirInstalacionResponse>(jsonResponse);
                if (response.success)
                {
                    Debug.Log($"Instalación {response.instalacionId} construida. Solaris restantes: {response.solarisRestantes}");
                    // Actualizar la UI con los nuevos Solaris y la nueva instalación
                }
                else
                {
                    Debug.LogError($"Fallo al construir instalación: {response.message}");
                }
            }
        }
    */


    public IEnumerator ConstruirInstalacion(
        Guid partidaId,
        Guid enclaveId,
        string codigo,
        Action onSuccess,
        Action<string> onError)
    {
        ConstruirInstalacionRequest request =
            new ConstruirInstalacionRequest
            {
                PartidaId = partidaId,
                EnclaveId = enclaveId,
                Codigo = codigo
            };

        string json = JsonConvert.SerializeObject(request);

        Debug.Log("JSON construir instalación: " + json);

        using (UnityWebRequest webRequest =
            new UnityWebRequest(
                BASE_URL + "/construir-instalacion",
                "POST"))
        {
            byte[] bodyRaw = Encoding.UTF8.GetBytes(json);

            webRequest.uploadHandler = new UploadHandlerRaw(bodyRaw);
            webRequest.downloadHandler = new DownloadHandlerBuffer();

            webRequest.SetRequestHeader("Content-Type", "application/json");

            yield return webRequest.SendWebRequest();

            if (webRequest.result == UnityWebRequest.Result.Success)
            {
                Debug.Log("Instalación guardada correctamente en API.");
                onSuccess?.Invoke();
            }
            else
            {
                Debug.LogError("Error API construir instalación: " + webRequest.downloadHandler.text);
                onError?.Invoke(webRequest.downloadHandler.text);
            }
        }
    }


}
