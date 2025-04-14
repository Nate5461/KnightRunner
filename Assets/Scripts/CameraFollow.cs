using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{

    public Transform target; // The player character
    public float smoothSpeed = 0.125f; // Speed of camera smoothing
    public Vector3 offset; // Offset to position the camera
    
    void Start()
    {
        // Ensure the camera starts with a proper position
        transform.position = target.position + offset;
    }

    void LateUpdate()
    {
        if (target != null)
        {
            Vector3 desiredPosition = target.position + offset;
            Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
            transform.position = smoothedPosition;
        }
    }
}
