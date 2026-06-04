using UnityEngine;
using UnityEngine.EventSystems;

public class MovCamara : MonoBehaviour
{
    [Header("Zoom")]
    [SerializeField] private float zoomSpeed = 5f;
    [SerializeField] private float minZoom = 1f;
    [SerializeField] private float maxZoom = 40f;

    [Header("Mapa activo")]
    [SerializeField] private SpriteRenderer mapRenderer;

    private Camera cam;
    private Vector3 dragOrigin;

    private float minX;
    private float maxX;
    private float minY;
    private float maxY;

    private void Start()
    {
        cam = Camera.main;

        // Si el mapa ya está asignado desde el Inspector, lo usa.
        // Si no, GameMapManager se lo asignará después con SetMapRenderer().
        if (mapRenderer != null)
        {
            CalculateMapLimits();
            CenterCameraOnMap();
            cam.orthographicSize = maxZoom;
            ClampCamera();
        }
    }

    private void Update()
    {
        HandleDrag();
        HandleZoom();
        ClampCamera();
    }

    private void HandleDrag()
    {
        if (cam == null)
            return;

        // Evita mover la cámara si el ratón está encima de un botón/UI
        if (EventSystem.current != null && EventSystem.current.IsPointerOverGameObject())
            return;

        // Botón central del ratón
        if (Input.GetMouseButtonDown(2))
        {
            dragOrigin = cam.ScreenToWorldPoint(Input.mousePosition);
        }

        if (Input.GetMouseButton(2))
        {
            Vector3 currentPos = cam.ScreenToWorldPoint(Input.mousePosition);
            Vector3 difference = dragOrigin - currentPos;

            transform.position += new Vector3(
                difference.x,
                difference.y,
                0f
            );
        }
    }

    private void HandleZoom()
    {
        if (cam == null)
            return;

        float scroll = Input.mouseScrollDelta.y;

        if (Mathf.Abs(scroll) < 0.01f)
            return;

        cam.orthographicSize -= scroll * zoomSpeed;

        cam.orthographicSize = Mathf.Clamp(
            cam.orthographicSize,
            minZoom,
            maxZoom
        );
    }

    private void CalculateMapLimits()
    {
        if (mapRenderer == null)
            return;

        Bounds bounds = mapRenderer.bounds;

        minX = bounds.min.x;
        maxX = bounds.max.x;
        minY = bounds.min.y;
        maxY = bounds.max.y;
    }

    private void CenterCameraOnMap()
    {
        if (mapRenderer == null)
            return;

        Vector3 center = mapRenderer.bounds.center;

        transform.position = new Vector3(
            center.x,
            center.y,
            transform.position.z
        );
    }

    private void ClampCamera()
    {
        if (cam == null || mapRenderer == null)
            return;

        float cameraHeight = cam.orthographicSize;
        float cameraWidth = cameraHeight * cam.aspect;

        float mapWidth = maxX - minX;
        float mapHeight = maxY - minY;

        float clampedX;
        float clampedY;

        if (cameraWidth * 2f >= mapWidth)
        {
            clampedX = (minX + maxX) * 0.5f;
        }
        else
        {
            clampedX = Mathf.Clamp(
                transform.position.x,
                minX + cameraWidth,
                maxX - cameraWidth
            );
        }

        if (cameraHeight * 2f >= mapHeight)
        {
            clampedY = (minY + maxY) * 0.5f;
        }
        else
        {
            clampedY = Mathf.Clamp(
                transform.position.y,
                minY + cameraHeight,
                maxY - cameraHeight
            );
        }

        transform.position = new Vector3(
            clampedX,
            clampedY,
            transform.position.z
        );
    }

    public void SetMapRenderer(SpriteRenderer newMapRenderer)
    {
        mapRenderer = newMapRenderer;

        if (cam == null)
        {
            cam = Camera.main;
        }

        if (mapRenderer == null)
        {
            Debug.LogError("No se puede asignar el mapa a la cámara porque el MapRenderer es null.");
            return;
        }

        CalculateMapLimits();

        CenterCameraOnMap();

        cam.orthographicSize = maxZoom;

        ClampCamera();
    }
}