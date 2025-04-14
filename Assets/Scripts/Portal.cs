using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Portal : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Check if the colliding object has the Player tag
        if (collision.CompareTag("Player"))
        {
            // Call the EndGame method from GameController
            GameController.instance.EndGame();

            // Optionally destroy the apple object after the collision
            Destroy(gameObject);
        }
    }
}
