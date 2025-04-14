using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class NecroController : MonoBehaviour
{
    public float maxHealth = 100f;
    public float walkSpeed = 3f;
    public float runSpeed = 6f;
    public float jumpForce = 5f;
    public Rigidbody2D rb;
    public Animator animator;
    public Slider healthBar;

    private Vector3 initialScale;

    private bool isGrounded = true;
    private float horizontalSpeed; // Track horizontal speed when jumping
    private float currentHealth;

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
            animator.SetBool("isJumping", true);
            isGrounded = false;
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
            animator.SetBool("isJumping", false);
        }
    }
}
