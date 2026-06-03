using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovCamara : MonoBehaviour
{
    [Header("Zoom")]
    [SerializeField] private float zoomSpeed = 5f;
    [SerializeField] private float minZoom = 2f;
    [SerializeField] private float maxZoom = 20f;

    [Header("Map Limits")]
    [SerializeField] private float minX = 0f;
    [SerializeField] private float maxX = 200f;
    [SerializeField] private float minY = 0f;
    [SerializeField] private float maxY = 100f;

    private Camera cam;
    private Vector3 dragOrigin;

    private void Start()
    {
        cam = Camera.main;
    }

    private void Update()
    {
        HandleDrag();
        HandleZoom();
        ClampCamera();
    }

    private void HandleDrag()
    {
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

    private void ClampCamera()
    {
        float cameraHeight = cam.orthographicSize;
        float cameraWidth = cameraHeight * cam.aspect;

        float mapWidth = maxX - minX;
        float mapHeight = maxY - minY;

        float clampedX;
        float clampedY;

        // Si el mapa es más pequeño que lo que ve la cámara
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
}
