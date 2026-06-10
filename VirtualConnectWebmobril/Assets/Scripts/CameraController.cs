using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.EventSystems;

using VirtualConnectUtils.Act;


public class CameraController : MonoBehaviour
{
    [Header("Rotation Settings")]
    public float rotationSpeed = 80f;
 
    [Header("Zoom Settings (FOV)")]
    public float zoomSpeed = 30f;
    public float minFOV = 15f;
    public float maxFOV = 90f;
 
    private Camera cam;
 
    void Start()
    {
        cam = GetComponent<Camera>();
        if (cam == null)
        {
            Debug.LogWarning("CameraController: No Camera component found on this GameObject.");
        }
    }
 
    void Update()
    {
        HandleRotation();
        HandleZoom();
    }
 
    void HandleRotation()
    {
        float direction = 0f;
 
        if (Input.GetKey(KeyCode.A)) direction = -1f;
        else if (Input.GetKey(KeyCode.D)) direction =  1f;
 
        if (direction != 0f)
        {
            // Rotate around the world Y axis in place (position never changes)
            transform.Rotate(Vector3.up, direction * rotationSpeed * Time.deltaTime, Space.World);
        }
    }
 
    void HandleZoom()
    {
         if (EventSystem.current != null && EventSystem.current.IsPointerOverGameObject())
         return;

     float direction = 0f;
     if (Input.GetKey(KeyCode.W)) direction = -1f;
     else if (Input.GetKey(KeyCode.S)) direction = 1f;

     if (direction == 0f || cam == null) return;

        if (cam.orthographic)
        {
            cam.orthographicSize = Mathf.Clamp(
                cam.orthographicSize + direction * zoomSpeed * Time.deltaTime,
                minFOV, maxFOV
            );
        }
        else
        {
            cam.fieldOfView = Mathf.Clamp(
                cam.fieldOfView + direction * zoomSpeed * Time.deltaTime,
                minFOV, maxFOV
            );
        }

        // Force canvas to update its raycaster after FOV shift
        UpdateCanvasCamera();
    }

    void UpdateCanvasCamera()
{
    Canvas[] allCanvas = FindObjectsByType<Canvas>(FindObjectsSortMode.None);
    foreach (Canvas c in allCanvas)
    {
        if (c.renderMode == RenderMode.WorldSpace)
        {
            c.worldCamera = cam; // reassign to force raycaster refresh
        }
    }
}
}
