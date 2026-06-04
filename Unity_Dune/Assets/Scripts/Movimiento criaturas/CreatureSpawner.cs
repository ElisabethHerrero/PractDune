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
    public GameObject[] creaturePrefabs;

    public void SpawnRandomCreature()
    {
        if (creaturePrefabs == null || creaturePrefabs.Length == 0)
        {
            Debug.LogWarning("No hay prefabs de criaturas asignados.");
            return;
        }

        GameObject prefab = creaturePrefabs[UnityEngine.Random.Range(0, creaturePrefabs.Length)];
        SpawnCreature(prefab);
    }

    public GameObject SpawnCreature(GameObject creaturePrefab)
    {
        if (creaturePrefab == null)
        {
            Debug.LogWarning("El prefab de criatura es null.");
            return null;
        }

        Vector3Int? freeCell = GetRandomFreeCell();

        if (freeCell == null)
        {
            Debug.LogWarning("No hay celdas libres para generar criatura.");
            return null;
        }

        Vector3 spawnPosition = grid.GetCellCenterWorld(freeCell.Value);
        spawnPosition.z = 0f;

        GameObject newCreature = Instantiate(
            creaturePrefab,
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

        Debug.Log("Criatura generada en celda: " + freeCell.Value);

        return newCreature;
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
}