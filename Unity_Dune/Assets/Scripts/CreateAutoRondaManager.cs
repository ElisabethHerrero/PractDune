using System;
using System.Collections;
using UnityEngine;

public class AutoRondaManager : MonoBehaviour
{
    [Header("Configuración")]
    public float segundosEntreRondas = 10f;
    public bool ejecutarAutomaticamenteAlIniciar = false;

    [Header("Referencias")]
    public CreatureSpawner creatureSpawner;

    private bool rondasActivas = false;
    private Coroutine rutinaRondas;

    public GameManager gameManager;

    private void Start()
    {
        if (ejecutarAutomaticamenteAlIniciar)
        {
            IniciarRondasAutomaticas();
        }
    }

    public void IniciarRondasAutomaticas()
    {
        if (rondasActivas)
            return;

        rondasActivas = true;
        rutinaRondas = StartCoroutine(RutinaRondas());
    }

    public void DetenerRondasAutomaticas()
    {
        rondasActivas = false;

        if (rutinaRondas != null)
        {
            StopCoroutine(rutinaRondas);
            rutinaRondas = null;
        }
    }

    private IEnumerator RutinaRondas()
    {
        while (rondasActivas)
        {
            yield return new WaitForSeconds(segundosEntreRondas);

            EjecutarRonda();
        }
    }

    public void EjecutarRonda()
    {
        Guid partidaId = InfoPartida.CurrentGameId;

        if (partidaId == Guid.Empty)
        {
            Debug.LogError("No hay partida activa para ejecutar ronda.");
            DetenerRondasAutomaticas();
            return;
        }

        StartCoroutine(APICliente.Instance.EjecutarRonda(
            partidaId,
            onSuccess: (partidaActualizada) =>
            {
                Debug.Log("Ronda automática ejecutada.");

                gameManager.Solaris = partidaActualizada.Solaris;
                gameManager.NombrePartida = partidaActualizada.NombreJugador;
                gameManager.Enclaves = partidaActualizada.Enclaves;

                if (creatureSpawner != null)
                {
                    creatureSpawner.SpawnCreaturesFromPartida(partidaActualizada);
                }
            },
            onError: (error) =>
            {
                Debug.LogError("Error al ejecutar ronda automática: " + error);
            }
        ));
    }
}








