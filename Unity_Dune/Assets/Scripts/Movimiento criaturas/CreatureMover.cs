using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class CreatureMover : MonoBehaviour
{
    [Header("Referencias")]
    public Grid grid;
    public Tilemap walkableTilemap;
    public BuildingPlacer buildingPlacer;

    [Header("Movimiento")]
    public float moveSpeed = 2f;
    public float minWaitTime = 0.2f;
    public float maxWaitTime = 1.2f;

    private Vector3Int currentCell;
    private Vector3Int targetCell;
    private bool hasTarget;
    private float waitTimer;

    private SpriteRenderer spriteRenderer;

    private readonly Vector3Int[] directions =
    {
        new Vector3Int(1, 0, 0),
        new Vector3Int(-1, 0, 0),
        new Vector3Int(0, 1, 0),
        new Vector3Int(0, -1, 0)
    };

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();

        if (grid == null || walkableTilemap == null)
        {
            Debug.LogError("CreatureMover necesita Grid y WalkableTilemap asignados.");
            enabled = false;
            return;
        }

        currentCell = grid.WorldToCell(transform.position);
        transform.position = GetCellCenter(currentCell);

        ChooseNewTargetCell();
    }

    private void Update()
    {
        if (!hasTarget)
        {
            waitTimer -= Time.deltaTime;

            if (waitTimer <= 0f)
            {
                ChooseNewTargetCell();
            }

            return;
        }

        if (!IsCellFree(targetCell))
        {
            hasTarget = false;
            waitTimer = 0f;
            return;
        }

        Vector3 targetPosition = GetCellCenter(targetCell);

        transform.position = Vector3.MoveTowards(
            transform.position,
            targetPosition,
            moveSpeed * Time.deltaTime
        );

        if (Vector3.Distance(transform.position, targetPosition) < 0.01f)
        {
            currentCell = targetCell;
            hasTarget = false;
            waitTimer = UnityEngine.Random.Range(minWaitTime, maxWaitTime);
        }
    }

    private void ChooseNewTargetCell()
    {
        List<Vector3Int> possibleCells = new List<Vector3Int>();

        foreach (Vector3Int direction in directions)
        {
            Vector3Int possibleCell = currentCell + direction;

            if (IsCellFree(possibleCell))
            {
                possibleCells.Add(possibleCell);
            }
        }

        if (possibleCells.Count == 0)
        {
            hasTarget = false;
            waitTimer = UnityEngine.Random.Range(minWaitTime, maxWaitTime);
            return;
        }

        targetCell = possibleCells[UnityEngine.Random.Range(0, possibleCells.Count)];
        hasTarget = true;

        FlipSpriteIfNeeded(targetCell - currentCell);
    }

    private bool IsCellFree(Vector3Int cell)
    {
        if (!walkableTilemap.HasTile(cell))
            return false;

        if (buildingPlacer != null && buildingPlacer.IsCellOccupied(cell))
            return false;

        return true;
    }

    private Vector3 GetCellCenter(Vector3Int cell)
    {
        Vector3 position = grid.GetCellCenterWorld(cell);
        position.z = transform.position.z;
        return position;
    }

    private void FlipSpriteIfNeeded(Vector3Int direction)
    {
        if (spriteRenderer == null)
            return;
        if (direction.x > 0)
            spriteRenderer.flipX = true;
        else if (direction.x < 0)
            spriteRenderer.flipX = false;
    }
}