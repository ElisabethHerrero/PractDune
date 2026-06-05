using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    [SerializeField] private APICliente apiCliente;

    public static GameManager Instance { get; private set; }

    //lo de la partida
    public double Solaris;
    public string NombrePartida;
    public List<EnclaveDTO> Enclaves { get; private set; } = new List<EnclaveDTO>();
    public List<InstalacionDTO> Instalaciones;

    public EnclaveDTO EnclaveActual { get; private set; }
    public bool PartidaCargada { get; private set; }

    public Guid CurrentEnclaveId { get; private set; }

    private void Awake()
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

    // Start is called before the first frame update
    void Start()
    {
        InicializarPartidaDesdeID();
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void InicializarPartidaDesdeID()
    {
        // 1. Obtenemos el ID de tu clase estática (asumiendo que se llama InfoPartida.IdActual)
        Guid idACargar = InfoPartida.CurrentGameId;

        if (idACargar == Guid.Empty)
        {
            Debug.LogError("No hay un ID de partida guardado en InfoPartida");
            return;
        }

        // 2. Llamamos a la API
        StartCoroutine(apiCliente.ObtenerDetallePartida(idACargar,
            onSuccess: (datos) => {
                // 3. LLENAMOS LOS ATRIBUTOS DEL GAMEMANAGER
                this.Solaris = datos.Solaris;
                this.NombrePartida = datos.NombreJugador;
                this.Enclaves = datos.Enclaves;

                // Si quieres extraer todas las instalaciones de todos los enclaves:
                this.Instalaciones = new List<InstalacionDTO>();
                foreach (var enclave in datos.Enclaves)
                {
                    this.Instalaciones.AddRange(enclave.Instalaciones);
                }

                Debug.Log("GameManager actualizado con los datos de la partida: " + idACargar);

                // Aquí podrías disparar un evento de "UI_Actualizada" o cargar la siguiente escena
            },
            onError: (error) => {
                Debug.LogError("Error al cargar el detalle: " + error);
            }
        ));
    }
    public void SetCurrentGameAndEnclave(Guid gameId, Guid initialEnclaveId, List<EnclaveDTO> enclaves)
    {
        CurrentEnclaveId = gameId;
        CurrentEnclaveId = initialEnclaveId;
        Enclaves = enclaves;
        Debug.Log($"Partida {gameId} cargada. Enclave activo: {initialEnclaveId}");
    }

    public void SelectEnclave(Guid enclaveId)
    {
        if (Enclaves.Exists(e => e.Id == enclaveId))
        {
            CurrentEnclaveId = enclaveId;
            Debug.Log($"Enclave seleccionado: {enclaveId}");
        }
        else
        {
            Debug.LogWarning($"Intento de seleccionar un enclave no existente: {enclaveId}");
        }
    }


}
