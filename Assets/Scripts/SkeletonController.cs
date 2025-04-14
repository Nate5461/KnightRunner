using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkeletonController : MonoBehaviour
{
    public Transform player; // Assign the player's transform in the Inspector
    public float detectionRange = 5f;
    public float attackRange = 1.5f;
    public float moveSpeed = 2f;
    public float edgeDetectionDistance = 0.5f;
   
    public float maxHealth = 100f;
    public Slider healthBar;
    private float currentHealth;

    private Animator animator;
    private bool isAttacking = false;
    
    
       private void Start()
    {
        animator = GetComponent<Animator>();
        if (animator == null)
        {
            Debug.LogError("Animator component not found!");
        }

        currentHealth = maxHealth;

        if (healthBar != null)
        {
            healthBar.maxValue = maxHealth;
            healthBar.value = currentHealth;
        }
        else
        {
            Debug.LogError("HealthBar reference not set!");
        }

        if (player == null)
        {
            Debug.LogError("Player reference not set!");
        }
    }
    

   private void Update()
    {
        float distanceToPlayer = Vector2.Distance(transform.position, player.position);

        if (distanceToPlayer <= detectionRange && !isAttacking)
        {
            if (distanceToPlayer > attackRange)
            {
                // Move toward the player
                animator.SetBool("isWalking", true);
                MoveTowardPlayer();
            }
            else
            {
                // Attack the player
                animator.SetBool("isWalking", false);
                StartCoroutine(AttackPlayer());
            }
        }
        else
        {
            if (!isAttacking)
            {
                animator.SetBool("isWalking", false);
            }
        }

        if (!IsGroundAhead())
        {
            animator.SetBool("isWalking", false);
        }
    }


   private void MoveTowardPlayer()
    {
        Vector3 direction = (player.position - transform.position).normalized;

        // Check if there is ground ahead before moving
        if (IsGroundAhead())
        {
            transform.position += new Vector3(direction.x, 0, 0) * moveSpeed * Time.deltaTime;

            // Flip the skeleton to face the player
            if (direction.x > 0)
                transform.localScale = new Vector3(1, 1, 1);
            else
                transform.localScale = new Vector3(-1, 1, 1);
        }
        else
        {
            // Stop moving if there is no ground ahead
            animator.SetBool("isWalking", false);
        }
    }

    private System.Collections.IEnumerator AttackPlayer()
    {
        isAttacking = true;
        animator.SetTrigger("Attack");

        // Wait for the attack animation to complete (adjust time to match the animation length)
        yield return new WaitForSeconds(1f);

        // Check if the player is still in range to deal damage
        float distanceToPlayer = Vector2.Distance(transform.position, player.position);
        if (distanceToPlayer <= attackRange)
        {
            player.GetComponent<PlayerController>().TakeDamage(10f);
        }

        isAttacking = false;
    }

    private bool IsGroundAhead()
    {
        Vector2 position = transform.position;
        Vector2 direction = Vector2.down;
        float distance = edgeDetectionDistance;

        // Cast a ray down from the skeleton's feet slightly ahead of its current position
        RaycastHit2D hit = Physics2D.Raycast(position + Vector2.right * transform.localScale.x * 0.5f, direction, distance);

        // Debug visualization
        Debug.DrawRay(position + Vector2.right * transform.localScale.x * 0.5f, direction * distance, Color.red);

        return hit.collider != null;
    }

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        healthBar.value = currentHealth;

        if (currentHealth > 0)
        {
            animator.SetTrigger("takeHitTrigger");
        }
        else
        {
            Die();
        }
    }

    private void Die()
    {
        animator.SetTrigger("dieTrigger");
        // Disable the goblin's movement and attacks
        this.enabled = false;
        // Optionally, destroy the goblin after the death animation
        Destroy(gameObject, 2f); // Adjust the delay to match the length of the death animation
    }
}
