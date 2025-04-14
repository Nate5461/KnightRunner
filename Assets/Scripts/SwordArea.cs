using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordArea : MonoBehaviour
{
    

    private void OnTriggerEnter2D(Collider2D collision)
    {
       if (collision.CompareTag("Enemy_skeleton")) // Adjust tag
            {
                SkeletonController enemy = collision.GetComponent<SkeletonController>();
                if (enemy != null)
                {
                    Debug.Log("Skeleton hit");
                    enemy.TakeDamage(30f); // Adjust damage as needed
                }
            }
            else if (collision.CompareTag("Enemy_goblin")) // Adjust tag
            {
                GoblinController enemy = collision.GetComponent<GoblinController>();
                if (enemy != null)
                {
                    Debug.Log("Goblin hit");
                    enemy.TakeDamage(25f); // Adjust damage as needed
                }
            }
    }
}
