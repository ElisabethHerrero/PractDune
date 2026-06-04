using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Tilemaps;

public class BuildingPlacer : MonoBehaviour
{
    [Header("Referencias")]
    public Grid grid;
    public Tilemap buildTilemap;

    [Header("Instalación seleccionada")]
    public GameObject selectedBuildingPrefab;

    private GameObject previewBuilding;
    private BuildingVisualData previewData;

    private HashSet<Vector3Int> occupiedCells = new HashSet<Vector3Int>();

    void Update()
    {
        UpdatePreviewPosition();

        if (Input.GetMouseButtonDown(0))
        {
            // Evita construir al hacer click encima de un botón UI
            if (EventSystem.current != null && EventSystem.current.IsPointerOverGameObject())
                return;

            TryPlaceBuilding();
        }

        // Click derecho para cancelar la construcción
        if (Input.GetMouseButtonDown(1))
        {
            CancelSelection();
        }
    }

    public void SelectBuilding(GameObject buildingPrefab)
    {
        selectedBuildingPrefab = buildingPrefab;

        if (previewBuilding != null)
            Destroy(previewBuilding);

        previewBuilding = Instantiate(selectedBuildingPrefab);
        previewData = previewBuilding.GetComponent<BuildingVisualData>();

        if (previewData == null)
        {
            Debug.LogError("El prefab no tiene BuildingVisualData.");
            Destroy(previewBuilding);
            previewBuilding = null;
            selectedBuildingPrefab = null;
            return;
        }

        Collider2D collider = previewBuilding.GetComponent<Collider2D>();

        if (collider != null)
            collider.enabled = false;

        SpriteRenderer spriteRenderer = previewBuilding.GetComponent<SpriteRenderer>();

        if (spriteRenderer != null)
        {
            spriteRenderer.sortingLayerName = "Default";
            spriteRenderer.sortingOrder = 10;
            spriteRenderer.color = new Color(1f, 1f, 1f, 0.6f);
        }

        UpdatePreviewPosition();
    }

    void UpdatePreviewPosition()
    {
        if (previewBuilding == null || previewData == null)
            return;

        Vector3 mouseWorldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mouseWorldPosition.z = 0;

        Vector3Int originCell = grid.WorldToCell(mouseWorldPosition);

        Vector2Int size = previewData.GetSizeInCells(grid.cellSize);

        Vector3 previewPosition = GetCenteredWorldPosition(originCell, size);

        previewBuilding.transform.position = previewPosition;

        bool canBuild = CanBuild(originCell, size);
        SetPreviewColor(canBuild);
    }

    void TryPlaceBuilding()
    {
        if (selectedBuildingPrefab == null || previewData == null)
            return;

        Vector3 mouseWorldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mouseWorldPosition.z = 0;

        Vector3Int originCell = grid.WorldToCell(mouseWorldPosition);

        Vector2Int size = previewData.GetSizeInCells(grid.cellSize);

        if (!CanBuild(originCell, size))
        {
            Debug.Log("No se puede construir aquí.");
            return;
        }

        Vector3 spawnPosition = GetCenteredWorldPosition(originCell, size);

        GameObject newBuilding = Instantiate(selectedBuildingPrefab, spawnPosition, Quaternion.identity);

        SpriteRenderer spriteRenderer = newBuilding.GetComponent<SpriteRenderer>();

        if (spriteRenderer != null)
        {
            spriteRenderer.sortingLayerName = "Default";
            spriteRenderer.sortingOrder = 10;
            spriteRenderer.color = Color.white;
        }

        MarkCellsAsOccupied(originCell, size);

        Debug.Log("Instalación construida: " + previewData.codigoInstalacion);

        // Importante:
        // después de construir una instalación, se cancela la selección.
        // Así tienes que volver a pulsar el botón para construir otra.
        CancelSelection();
    }

    bool CanBuild(Vector3Int originCell, Vector2Int size)
    {
        for (int x = 0; x < size.x; x++)
        {
            for (int y = 0; y < size.y; y++)
            {
                Vector3Int cell = originCell + new Vector3Int(x, y, 0);

                if (!buildTilemap.HasTile(cell))
                    return false;

                if (occupiedCells.Contains(cell))
                    return false;
            }
        }

        return true;
    }

    void MarkCellsAsOccupied(Vector3Int originCell, Vector2Int size)
    {
        for (int x = 0; x < size.x; x++)
        {
            for (int y = 0; y < size.y; y++)
            {
                Vector3Int cell = originCell + new Vector3Int(x, y, 0);
                occupiedCells.Add(cell);
            }
        }
    }

    Vector3 GetCenteredWorldPosition(Vector3Int originCell, Vector2Int size)
    {
        Vector3 bottomLeftCellCenter = grid.GetCellCenterWorld(originCell);

        float offsetX = (size.x - 1) * grid.cellSize.x / 2f;
        float offsetY = (size.y - 1) * grid.cellSize.y / 2f;

        return bottomLeftCellCenter + new Vector3(offsetX, offsetY, 0);
    }

    void SetPreviewColor(bool canBuild)
    {
        if (previewBuilding == null)
            return;

        SpriteRenderer spriteRenderer = previewBuilding.GetComponent<SpriteRenderer>();

        if (spriteRenderer == null)
            return;

        if (canBuild)
        {
            // Blanco semitransparente: se puede construir
            spriteRenderer.color = new Color(1f, 1f, 1f, 0.6f);
        }
        else
        {
            // Rojo semitransparente: no se puede construir
            spriteRenderer.color = new Color(1f, 0.25f, 0.25f, 0.6f);
        }
    }

    void CancelSelection()
    {
        selectedBuildingPrefab = null;
        previewData = null;

        if (previewBuilding != null)
        {
            Destroy(previewBuilding);
            previewBuilding = null;
        }
    }

    public bool IsCellOccupied(Vector3Int cell)
    {
        return occupiedCells.Contains(cell);
    }

    public void SetActiveMap(Grid newGrid, Tilemap newBuildTilemap)
    {
        grid = newGrid;
        buildTilemap = newBuildTilemap;

        occupiedCells.Clear();

        CancelSelection();
    }
}