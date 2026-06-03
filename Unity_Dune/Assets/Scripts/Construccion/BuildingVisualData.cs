using UnityEngine;

public enum TipoRecintoVisual
{
    RocaSellada,
    EscudoEstatico,
    CupulaBlindada,
    PozoReforzado
}

[ExecuteAlways]
public class BuildingVisualData : MonoBehaviour
{
    [Header("Tipo visual")]
    public TipoRecintoVisual tipoRecinto;

    [Header("Código de instalación")]
    public string codigoInstalacion;

    [Header("Tamaño")]
    public bool calcularTamanoAutomaticamente = true;

    [Tooltip("Sólo se usa si calcularTamanoAutomaticamente está desactivado.")]
    public Vector2Int sizeInCells = new Vector2Int(1, 1);

    [Header("Gizmo de ayuda")]
    public bool dibujarRecuadro = true;

    [Tooltip("Pon aquí el mismo Cell Size que tenga tu Grid.")]
    public Vector2 cellSizeParaGizmo = new Vector2(1, 1);

    public Vector2Int GetSizeInCells(Vector3 gridCellSize)
    {
        if (!calcularTamanoAutomaticamente)
        {
            return sizeInCells;
        }

        return CalcularSizeInCells(gridCellSize);
    }

    private Vector2Int CalcularSizeInCells(Vector3 gridCellSize)
    {
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();

        if (spriteRenderer == null || spriteRenderer.sprite == null)
        {
            return sizeInCells;
        }

        float cellWidth = Mathf.Abs(gridCellSize.x);
        float cellHeight = Mathf.Abs(gridCellSize.y);

        if (cellWidth <= 0) cellWidth = 1;
        if (cellHeight <= 0) cellHeight = 1;

        Bounds bounds = spriteRenderer.bounds;

        int widthInCells = Mathf.Max(1, Mathf.CeilToInt(bounds.size.x / cellWidth));
        int heightInCells = Mathf.Max(1, Mathf.CeilToInt(bounds.size.y / cellHeight));

        return new Vector2Int(widthInCells, heightInCells);
    }

    private void OnValidate()
    {
        codigoInstalacion = ObtenerCodigoPorTipo(tipoRecinto);

        if (calcularTamanoAutomaticamente)
        {
            sizeInCells = CalcularSizeInCells(new Vector3(cellSizeParaGizmo.x, cellSizeParaGizmo.y, 0));
        }
    }

    private string ObtenerCodigoPorTipo(TipoRecintoVisual tipo)
    {
        switch (tipo)
        {
            case TipoRecintoVisual.RocaSellada:
                return "ADR05";

            case TipoRecintoVisual.EscudoEstatico:
                return "ADP03";

            case TipoRecintoVisual.CupulaBlindada:
                return "AAV02";

            case TipoRecintoVisual.PozoReforzado:
                return "ASU04";

            default:
                return "";
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (!dibujarRecuadro)
            return;

        Vector2Int size = calcularTamanoAutomaticamente
            ? CalcularSizeInCells(new Vector3(cellSizeParaGizmo.x, cellSizeParaGizmo.y, 0))
            : sizeInCells;

        Vector3 totalSize = new Vector3(
            size.x * cellSizeParaGizmo.x,
            size.y * cellSizeParaGizmo.y,
            0
        );

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(transform.position, totalSize);

        Gizmos.color = new Color(1f, 1f, 0f, 0.25f);

        float startX = transform.position.x - totalSize.x / 2f;
        float startY = transform.position.y - totalSize.y / 2f;

        for (int x = 0; x <= size.x; x++)
        {
            float lineX = startX + x * cellSizeParaGizmo.x;

            Gizmos.DrawLine(
                new Vector3(lineX, startY, transform.position.z),
                new Vector3(lineX, startY + totalSize.y, transform.position.z)
            );
        }

        for (int y = 0; y <= size.y; y++)
        {
            float lineY = startY + y * cellSizeParaGizmo.y;

            Gizmos.DrawLine(
                new Vector3(startX, lineY, transform.position.z),
                new Vector3(startX + totalSize.x, lineY, transform.position.z)
            );
        }
    }
}