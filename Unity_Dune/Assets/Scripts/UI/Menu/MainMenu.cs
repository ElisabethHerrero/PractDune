using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System.Text;
using TMPro; // Para TextMeshPro, si lo usas para mostrar la lista
using Newtonsoft.Json;
using System;
using UnityEditor.PackageManager;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    public GameObject CanvaInicio;
    public GameObject CanvaNuevaPartida;

    [SerializeField] public TMP_InputField nombre;
    [SerializeField] private Button Arrakeen;
    [SerializeField] private Button Caladan;
    [SerializeField] private Button Gieldi;

    public GameObject panelCargarPartida;
    public Transform ContenedorPartidas;
    public GameObject prefabBotonPartida;

    public GameObject SinPartidas;

    private APICliente apiCliente;



    private void Awake()
    {
        apiCliente = GetComponent<APICliente>();
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

    public void AbrirCrearPartida()
    {
        CanvaNuevaPartida.SetActive(true);
        CanvaInicio.SetActive(false);

        nombre.interactable = true;

        Arrakeen.interactable = false;
        Caladan.interactable = false;
        Gieldi.interactable = false;

        nombre.onValueChanged.AddListener(ValidarInput);


    }

    public void CerrarCrearPartida()
    {
        CanvaNuevaPartida.SetActive(false);
    }



    private void OnListarPartidasSuccess(ListaPartidasResponse response)
    {
        SinPartidas.SetActive(false);

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
        SinPartidas.SetActive(true);

        Debug.LogError($"Error al listar partidas: {error}");
    }


    //

    private void ValidarInput(string texto)
    {
        Arrakeen.interactable =
            !string.IsNullOrWhiteSpace(texto) &&
            texto.Length >= 3;

        Caladan.interactable =
            !string.IsNullOrWhiteSpace(texto) &&
            texto.Length >= 3;
        Gieldi.interactable =
            !string.IsNullOrWhiteSpace(texto) &&
            texto.Length >= 3;
    }


    public void CrearArraken()
    {
        VisualSceneData.EscenarioSeleccionado = EscenarioJuego.Arrakeen;
        StartCoroutine(apiCliente.CrearPartida(
        nombre.text,
        EscenarioJuego.Arrakeen,
        ArrakeenExito,
        ArrakeenError
        ));
    }

    public void CrearCaladan()
    {
        VisualSceneData.EscenarioSeleccionado = EscenarioJuego.Caladan;
        StartCoroutine(apiCliente.CrearPartida(
        nombre.text,
        EscenarioJuego.Caladan,
        ArrakeenExito,
        ArrakeenError
        ));
    }

    public void CrearGiedi()
    {
        VisualSceneData.EscenarioSeleccionado = EscenarioJuego.GiediPrime;
        StartCoroutine(apiCliente.CrearPartida(
        nombre.text,
        EscenarioJuego.GiediPrime,
        ArrakeenExito,
        ArrakeenError
        ));
    }


    private void ArrakeenExito(PartidaResumenDTO partida)
    {
        SceneManager.LoadScene(1);
    }

    private void ArrakeenError(string error)
    {
        Debug.LogError("No se creo la partida");
    }


    public void CargarJuegoTemporal()
    {
        SceneManager.LoadScene(1);
    }










}
