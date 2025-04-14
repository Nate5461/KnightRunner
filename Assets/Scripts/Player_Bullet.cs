using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Bullet : MonoBehaviour
{
    public float speed = 5f;
    public float lifetime = 5f;
    public Vector2 direction = Vector2.right;
    public float damage = 25f;

    void Start()
    {
        Destroy(gameObject, lifetime); // Destroy bullet after a certain time
    }

    void Update()
    {
        transform.Translate(direction * speed * Time.deltaTime);
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy_skeleton")) // Check if the object has the "Enemy" tag
        {
            SkeletonController skeleton = collision.GetComponent<SkeletonController>();
            if (skeleton != null)
            {
                skeleton.TakeDamage(damage); 
            }
            Destroy(gameObject); // Destroy the bullet
        }
        else if (collision.CompareTag("Enemy_goblin")) // Check if the object has the "Enemy" tag
        {
            GoblinController goblin = collision.GetComponent<GoblinController>();
            if (goblin != null)
            {
                goblin.TakeDamage(damage); 
            }
            Destroy(gameObject); // Destroy the bullet
        }
        else if(!collision.CompareTag("Player")) 
        {
            if (collision.CompareTag("Ground"))
            {
                Destroy(gameObject); // Destroy the bullet if it hits the ground
            }
        }
    }

}
