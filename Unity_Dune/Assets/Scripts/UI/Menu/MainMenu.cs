using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System.Text;
using TMPro; // Para TextMeshPro, si lo usas para mostrar la lista
using Newtonsoft.Json;
using System;

public class MainMenu : MonoBehaviour
{
    public GameObject CanvaInicio;
    public GameObject CanvaNuevaPartida;

    public GameObject panelCargarPartida;
    public Transform ContenedorPartidas;
    public GameObject prefabBotonPartida;

    private APICliente apiCliente;


    private void Awake()
    {
        apiCliente = new APICliente();
    }

    // Start is called before the first frame update
    void Start()
    {
        CanvaInicio.SetActive(true);
        panelCargarPartida.SetActive(false);
        CanvaNuevaPartida.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnClickCargarPartida()
    {
        panelCargarPartida.SetActive(true);
        StartCoroutine(apiCliente.ListarPartidas(OnListarPartidasSuccess, OnListarPartidasError));
    }

    public void OnClickCerrarCargarPartida()
    {
        panelCargarPartida.SetActive(false);
    }



    private void OnListarPartidasSuccess(ListaPartidasResponse response)
    {
        Debug.Log($"Se encontraron {response.Partidas.Count} partidas");

        foreach (PartidaResumenDTO partida in response.Partidas)
        {
            GameObject boton =
                Instantiate(prefabBotonPartida, ContenedorPartidas);

            boton.GetComponentInChildren<TextMeshProUGUI>().text = partida.NombreJugador;

            Guid id = partida.Id;
            boton.GetComponent<UnityEngine.UI.Button>()
                 .onClick
                 .AddListener(() => StartCoroutine(apiCliente.CargarPartidaCo(id)));
        }
    }

    private void OnListarPartidasError(string error)
    {
        Debug.LogError($"Error al listar partidas: {error}");
    }


}
