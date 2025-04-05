using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] private Transform target;
    [SerializeField] private float smoothSpeed = 0.125f;
    [SerializeField] private Vector3 offset = new Vector3(0, 0, -10);
    [SerializeField] private float orthographicSize = 25f; // Appropriate zoom level for the game

    private Camera cam;

    private void Awake()
    {
        cam = GetComponent<Camera>();
        if (cam != null)
        {
            cam.orthographicSize = orthographicSize;
        }
    }

    private void Start()
    {
        // Automatically find player if target is not set
        if (target == null)
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if (player != null)
            {
                target = player.transform;
            }
            else
            {
                Debug.LogWarning("CameraFollow: No target assigned and no GameObject with tag 'Player' found.");
            }
        }
    }

    private void LateUpdate()
    {
        if (target == null)
            return;

        // Calculate desired position (maintain current Z position)
        Vector3 targetPosition = target.position;
        Vector3 desiredPosition = new Vector3(targetPosition.x, targetPosition.y, transform.position.z);
        
        // Smoothly move towards that position
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed * Time.deltaTime * 10f);
        transform.position = smoothedPosition;
    }

    // Method to adjust zoom level at runtime if needed
    public void SetZoom(float newSize)
    {
        if (cam != null)
        {
            cam.orthographicSize = newSize;
            orthographicSize = newSize;
        }
    }
}
