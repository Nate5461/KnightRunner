using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GoblinController : MonoBehaviour
{
    
    public float speed = 2.0f;
    public float attackRange = 1.0f;
    public float attackCooldown = 1.0f;
    public int damage = 10;
    public float maxHealth = 100;
    public Slider healthBar;

    private Transform player;
    private Animator animator;
    private float lastAttackTime;
    private float currentHealth;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        animator = GetComponent<Animator>();
        lastAttackTime = -attackCooldown;
        currentHealth = maxHealth;
        healthBar.maxValue = maxHealth;
        healthBar.value = currentHealth;
    }

    
    void Update()
    {
        if (player != null){
            
            float distanceToPlayer = Vector2.Distance(transform.position, player.position);

            if (distanceToPlayer > attackRange) 
            {    
                // Move towards the player
                Vector2 direction = (player.position - transform.position).normalized;
                transform.position = Vector2.MoveTowards(transform.position, player.position, speed * Time.deltaTime);
                animator.SetBool("isWalking", true);

                Vector3 localScale = transform.localScale;

                if (direction.x > 0)
                {
                    localScale.x = Mathf.Abs(localScale.x) * -1; // Facing right
                }
                else if (direction.x < 0)
                {
                    localScale.x = Mathf.Abs(localScale.x); // Facing left
                }

                transform.localScale = localScale;
            }
            else
            {
                // Attack the player
                animator.SetBool("isWalking", false);
                if (Time.time >= lastAttackTime + attackCooldown)
                {
                    Attack();
                    lastAttackTime = Time.time;
                }
            }
        }
    }

    void Attack()
    {
        animator.SetTrigger("attackTrigger");
        
        player.GetComponent<PlayerController>().TakeDamage(damage);
    }

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        healthBar.value = currentHealth;

        if (currentHealth > 0)
        {
            StartCoroutine(TakeHitCoroutine());
        }
        else
        {
            Die();
        }
    }

    private IEnumerator TakeHitCoroutine()
    {
        
        speed = 0;
        animator.SetTrigger("takeHitTrigger");
        yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length);

        speed = 2.0f;
        
    }

    private void Die()
    {
        animator.SetTrigger("dieTrigger");
        this.enabled = false;
        
        Destroy(gameObject, 2f);
    }
}
