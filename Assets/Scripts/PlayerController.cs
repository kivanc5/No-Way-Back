using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float walkSpeed = 1f;
    public float runSpeed = 1f;
    public float speed = 1f;
    public float jumpForce = 1f;

    public Transform cameraTransform;
    public Animator animator;

    private Rigidbody rb;
    private bool isGrounded;
    private bool isCrouching;

    public CapsuleCollider standingCollider;
    public CapsuleCollider crouchCollider;

    
    private Vector3 spawnPoint;

    void Start()
    {
        rb = GetComponent<Rigidbody>();

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        
        spawnPoint = transform.position;
    }

    void Update()
    {
        Move();
        Run();
        Jump();
        Crouch();
        UpdateAnimator();

        if (animator.GetBool("isRunning") == true)
        {
            speed = runSpeed;
        }
        else
        {
            speed = walkSpeed;
        }
    }

    void Move()
    {
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");

        Vector3 camForward = Vector3.Scale(
            cameraTransform.forward,
            new Vector3(1, 0, 1)
        ).normalized;

        Vector3 camRight = cameraTransform.right;

        Vector3 direction = camForward * v + camRight * h;
        direction = Vector3.ClampMagnitude(direction, 1f);

        rb.linearVelocity = new Vector3(
            direction.x * speed,
            rb.linearVelocity.y,
            direction.z * speed
        );

        if (direction != Vector3.zero)
        {
            transform.forward = direction;
        }

        animator.SetFloat("Speed", direction.magnitude * speed);
    }

    void Jump()
    {
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded && animator.GetBool("isCrouching") == false)
        {
            rb.linearVelocity = new Vector3(rb.linearVelocity.x, jumpForce, rb.linearVelocity.z);
            isGrounded = false;
            animator.SetBool("isGrounded", false);
            animator.SetTrigger("isJump");
        }
    }

    void Crouch()
    {
        if (Input.GetKeyDown(KeyCode.LeftControl) && isGrounded)
        {
            animator.SetBool("isCrouching", !animator.GetBool("isCrouching"));
        }
        else if (animator.GetBool("isCrouching") == true)
        {
            standingCollider.enabled = false;
            crouchCollider.enabled = true;
        }
        else
        {
            standingCollider.enabled = true;
            crouchCollider.enabled = false;
        }
    }

    void Run()
    {
        if (Input.GetKey(KeyCode.LeftShift) && animator.GetBool("isCrouching") == false)
        {
            animator.SetBool("isRunning", true);
        }
        else
        {
            animator.SetBool("isRunning", false);
        }
    }

    void UpdateAnimator()
    {
        animator.SetBool("isGrounded", isGrounded);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
        }
    }

    
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Lava"))
        {
            Die();
        }
    }

    
    void Die()
    {
        rb.linearVelocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;

        transform.position = spawnPoint;
    }
}
