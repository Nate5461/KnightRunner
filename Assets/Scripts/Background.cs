using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Background : MonoBehaviour
{
    private float length, startpos;
    public GameObject cam; // Reference to the camera
    public float effect = 0.1f; // Parallax effect strength

    void Start()
    {
        startpos = transform.position.x; // Initial position of the background
        length = GetComponent<SpriteRenderer>().bounds.size.x; // Length of the background sprite
    }

    void FixedUpdate()
    {
        float temp = cam.transform.position.x * (1 - effect); // Camera's relative position to the background
        float dist = cam.transform.position.x * effect; // Parallax distance

        // Apply parallax effect
        transform.position = new Vector3(startpos + dist, transform.position.y, transform.position.z);

        
    }
}