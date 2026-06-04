using System;
using UnityEngine;
using UnityEngine.Tilemaps;

[Serializable]
public class GameMapData
{
    public EscenarioJuego escenario;

    [Header("Objeto raíz del mapa")]
    public GameObject mapRoot;

    [Header("Fondo del mapa")]
    public SpriteRenderer mapRenderer;

    [Header("Grid y Tilemaps")]
    public Grid grid;
    public Tilemap buildTilemap;
    public Tilemap walkableTilemap;
}

public class GameMapManager : MonoBehaviour
{
    [Header("Mapas disponibles")]
    public GameMapData[] mapas;

    [Header("Sistemas de juego")]
    public BuildingPlacer buildingPlacer;
    public CreatureSpawner creatureSpawner;
    public MovCamara movCamara;

    private void Start()
    {
        AplicarMapaSeleccionado();
    }

    private void AplicarMapaSeleccionado()
    {
        EscenarioJuego escenarioSeleccionado = VisualSceneData.EscenarioSeleccionado;

        GameMapData mapaActivo = null;

        foreach (GameMapData mapa in mapas)
        {
            bool esElSeleccionado = mapa.escenario == escenarioSeleccionado;

            if (mapa.mapRoot != null)
            {
                mapa.mapRoot.SetActive(esElSeleccionado);
            }

            if (esElSeleccionado)
            {
                mapaActivo = mapa;
            }
        }

        if (mapaActivo == null)
        {
            Debug.LogError("No se encontró configuración para el mapa: " + escenarioSeleccionado);
            return;
        }

        Tilemap tilemapMovimiento = mapaActivo.walkableTilemap;

        if (tilemapMovimiento == null)
        {
            tilemapMovimiento = mapaActivo.buildTilemap;
        }

        if (buildingPlacer != null)
        {
            buildingPlacer.SetActiveMap(
                mapaActivo.grid,
                mapaActivo.buildTilemap
            );
        }

        if (creatureSpawner != null)
        {
            creatureSpawner.SetActiveMap(
                mapaActivo.grid,
                tilemapMovimiento,
                buildingPlacer
            );
        }

        if (movCamara != null)
        {
            movCamara.SetMapRenderer(mapaActivo.mapRenderer);
        }

        Debug.Log("Mapa visual cargado: " + escenarioSeleccionado);
    }
}