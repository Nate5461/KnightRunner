using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{

    public float maxHealth = 100f;
    public float walkSpeed = 3f;
    public float runSpeed = 6f;
    public float jumpForce = 5f;
    public float climbSpeed = 3f;
    public Rigidbody2D rb;
    public Animator animator;
    public Slider healthBar;
    public bool isOnLadder = false;

    private Vector3 initialScale;

    private bool isGrounded = true;
    private float horizontalSpeed;
    private float currentHealth;
    public GameObject bulletPrefab; 
    public Transform firePoint;
    public GameObject swordCollider;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        initialScale = transform.localScale;
        currentHealth = maxHealth;
        healthBar.maxValue = maxHealth;
        healthBar.value = currentHealth;
    }

    // Update is called once per frame
    void Update()
    {
        float move = Input.GetAxis("Horizontal");
        float verticalMove = Input.GetAxis("Vertical");

        if (isOnLadder)
        {
            rb.gravityScale = 0;
            rb.velocity = new Vector2(rb.velocity.x, verticalMove * climbSpeed);

        } else {
            rb.gravityScale = 1;
        }
        if (isGrounded)
        {
            // Set speed based on walk or run
            bool isRunning = Input.GetKey(KeyCode.LeftShift);
            horizontalSpeed = isRunning ? runSpeed : walkSpeed;
            rb.velocity = new Vector2(move * horizontalSpeed, rb.velocity.y);

            // Set animator parameters
            if (move != 0)
            {
                animator.SetBool("isWalking", !isRunning);
                animator.SetBool("isRunning", isRunning);
                animator.speed = isRunning ? 2f : 1f; // Adjust the speed of the animation
                transform.localScale = new Vector3(Mathf.Sign(move) * Mathf.Abs(initialScale.x), initialScale.y, initialScale.z);
            }
            else
            {
                animator.SetBool("isWalking", false);
                animator.SetBool("isRunning", false);
                animator.speed = 1f;
            }
        }
        else
        {
            // Apply saved horizontal speed when in the air
            rb.velocity = new Vector2(move * horizontalSpeed, rb.velocity.y);

            // Reset walking and running animations when in the air
            animator.SetBool("isWalking", false);
            animator.SetBool("isRunning", false);
        }

        if (move != 0)
        {
            transform.localScale = new Vector3(Mathf.Sign(move) * Mathf.Abs(initialScale.x), initialScale.y, initialScale.z);
        }

        // Jumping
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            animator.SetTrigger("jumpTrigger");
            isGrounded = false;
        }

        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            Shoot();
        }

        if (Input.GetKeyDown(KeyCode.F))
        {
            Debug.Log("Sword Attack");
            SwordAttack();
        }

    }

    void SwordAttack()
    {
        animator.SetTrigger("swordTrigger");
        
    }

    public void EnableSwordAttackCollider()
    {
        if (swordCollider != null)
        {
            swordCollider.SetActive(true); // Enable the collider
        }
    }

    public void DisableSwordAttackCollider()
    {
        if (swordCollider != null)
        {
            swordCollider.SetActive(false); // Disable the collider
        }
    }

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        healthBar.value = currentHealth;

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Shoot(){
        animator.SetTrigger("shootTrigger");
        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
        Player_Bullet bulletScript = bullet.GetComponent<Player_Bullet>();
        bulletScript.direction = transform.localScale.x > 0 ? Vector2.right : Vector2.left;

    }

    private void Die()
    {
        animator.SetTrigger("dieTrigger");
        rb.velocity = Vector2.zero;
        rb.isKinematic = true;

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void Heal(float amount)
    {
        currentHealth += amount;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        healthBar.value = currentHealth;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
        } else if (collision.gameObject.CompareTag("Finish"))
        {
            Debug.Log("Game Over");
            GameController.instance.EndGame();
        }
    }
}
