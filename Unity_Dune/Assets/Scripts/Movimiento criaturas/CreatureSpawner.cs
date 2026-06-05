using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class CreatureSpawner : MonoBehaviour
{
    [Header("Referencias")]
    public Grid grid;
    public Tilemap walkableTilemap;
    public BuildingPlacer buildingPlacer;

    [Header("Prefabs de criaturas")]
    public List<CreaturePrefabEntry> creaturePrefabs = new List<CreaturePrefabEntry>();

    private List<GameObject> spawnedCreatures = new List<GameObject>();

    public void SpawnCreaturesFromPartida(PartidaDetalleDTO partida)
    {
        ClearCreatures();

        if (partida == null || partida.Enclaves == null)
        {
            Debug.LogWarning("No hay partida o enclaves para generar criaturas visuales.");
            return;
        }

        foreach (EnclaveDTO enclave in partida.Enclaves)
        {
            foreach (InstalacionDTO instalacion in enclave.Instalaciones)
            {
                foreach (CriaturaDTO criatura in instalacion.Criaturas)
                {
                    SpawnCreatureFromDTO(criatura);
                }
            }
        }
    }

    private void SpawnCreatureFromDTO(CriaturaDTO criatura)
    {
        if (criatura == null)
            return;

        if (criatura.EstadoCriatura != EstadoCriatura.Activa)
            return;

        GameObject prefab = GetPrefabForCreature(criatura);

        if (prefab == null)
        {
            Debug.LogWarning("No hay prefab asignado para la criatura: " + criatura.Especie);
            return;
        }

        Vector3Int? freeCell = GetRandomFreeCell();

        if (freeCell == null)
        {
            Debug.LogWarning("No hay celdas libres para generar criatura.");
            return;
        }

        Vector3 spawnPosition = grid.GetCellCenterWorld(freeCell.Value);
        spawnPosition.z = 0f;

        GameObject newCreature = Instantiate(
            prefab,
            spawnPosition,
            Quaternion.identity
        );

        SpriteRenderer spriteRenderer = newCreature.GetComponent<SpriteRenderer>();

        if (spriteRenderer != null)
        {
            spriteRenderer.sortingLayerName = "Default";
            spriteRenderer.sortingOrder = 20;
            spriteRenderer.color = Color.white;
        }

        CreatureMover mover = newCreature.GetComponent<CreatureMover>();

        if (mover == null)
        {
            mover = newCreature.AddComponent<CreatureMover>();
        }

        mover.grid = grid;
        mover.walkableTilemap = walkableTilemap;
        mover.buildingPlacer = buildingPlacer;

        spawnedCreatures.Add(newCreature);

        Debug.Log("Criatura visual generada: " + criatura.Especie);
    }

    private GameObject GetPrefabForCreature(CriaturaDTO criatura)
    {
        CreaturePrefabEntry entry =
            creaturePrefabs.Find(x => x.especie == criatura.Especie);

        if (entry == null)
            return null;

        if (criatura.EdadActual < criatura.EdadAdulta)
            return entry.babyPrefab;

        return entry.adultPrefab;
    }

    private void ClearCreatures()
    {
        foreach (GameObject creature in spawnedCreatures)
        {
            if (creature != null)
                Destroy(creature);
        }

        spawnedCreatures.Clear();
    }

    private Vector3Int? GetRandomFreeCell()
    {
        if (grid == null || walkableTilemap == null)
        {
            Debug.LogError("CreatureSpawner necesita Grid y WalkableTilemap.");
            return null;
        }

        List<Vector3Int> freeCells = new List<Vector3Int>();

        BoundsInt bounds = walkableTilemap.cellBounds;

        foreach (Vector3Int position in bounds.allPositionsWithin)
        {
            Vector3Int cell = new Vector3Int(position.x, position.y, 0);

            if (IsCellFree(cell))
            {
                freeCells.Add(cell);
            }
        }

        if (freeCells.Count == 0)
            return null;

        return freeCells[UnityEngine.Random.Range(0, freeCells.Count)];
    }

    private bool IsCellFree(Vector3Int cell)
    {
        if (!walkableTilemap.HasTile(cell))
            return false;

        if (buildingPlacer != null && buildingPlacer.IsCellOccupied(cell))
            return false;

        return true;
    }

    public void SetActiveMap(Grid newGrid, Tilemap newWalkableTilemap, BuildingPlacer newBuildingPlacer)
    {
        grid = newGrid;
        walkableTilemap = newWalkableTilemap;
        buildingPlacer = newBuildingPlacer;
    }
}
