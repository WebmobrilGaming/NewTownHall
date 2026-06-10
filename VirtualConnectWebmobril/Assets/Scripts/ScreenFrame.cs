using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using VirtualConnectUtils.Act;

public class ScreenFrame : MonoBehaviour
{
   [Header("Click Settings")]
    public KeyCode clickButton = KeyCode.Mouse0;   // Left mouse button
    public LayerMask clickableLayers = ~0;          // All layers by default
    public float maxRayDistance = 100f;
 
    private Camera mainCam;
 
    void Start()
    {
        mainCam = Camera.main;
        if (mainCam == null)
            Debug.LogWarning("ClickableObject: No Camera tagged 'MainCamera' found.");
    }
 
    void Update()
    {
        if (Input.GetKeyDown(clickButton))
        {
            DetectClick();
        }
    }
 
    void DetectClick()
    {
        Ray ray = mainCam.ScreenPointToRay(Input.mousePosition);
 
        if (Physics.Raycast(ray, out RaycastHit hit, maxRayDistance, clickableLayers))
        {
            // Check if the ray hit THIS object specifically
            if (hit.collider.gameObject == gameObject)
            {
                OnObjectClicked(hit);
            }
        }
    }
 
    // -----------------------------------------------------------
    // Override or extend this method with your own logic
    // -----------------------------------------------------------
    void OnObjectClicked(RaycastHit hit)
    {
        Debug.Log($"Clicked: {gameObject.name} at {hit.point}");  

        //Actions.SetZoomTarget?.Invoke(transform);  // Example: Set this object as the zoom target
    }
 
    // Optional: Visual feedback using built-in Unity messages
    void OnMouseEnter()
    {
        // Cursor.SetCursor(hoverCursorTexture, Vector2.zero, CursorMode.Auto);
        Debug.Log($"Hovering over: {gameObject.name}");
    }
 
    void OnMouseExit()
    {
        // Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
    }
}
