using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("Move")]
    public float speed = 7.5f;

    [Header("Jump")]
    public float jumpForce = 20f;
    public float coyoteTime = 0.12f;
    public float jumpBuffer = 0.12f;

    [Header("Ground Check")]
    public Transform groundCheck;
    public float groundRadius = 0.2f;
    public LayerMask groundMask;

    Rigidbody2D rb;
    Animator anim;

    bool isGrounded;
    bool wasGrounded;

    float coyoteCounter;
    float bufferCounter;

    public float fallMultiplier = 2.2f;
    public float lowJumpMultiplier = 1.8f;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        if (!groundCheck) return;

        // simpan grounded sebelumnya (buat landing / transisi halus)
        wasGrounded = isGrounded;

        // grounded check
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundRadius, groundMask);

        // coyote timer
        if (isGrounded) coyoteCounter = coyoteTime;
        else coyoteCounter -= Time.deltaTime;

        // jump buffer
        if (Input.GetButtonDown("Jump")) bufferCounter = jumpBuffer;
        else bufferCounter -= Time.deltaTime;

        bool didJump = false;

        // execute jump
        if (bufferCounter > 0f && coyoteCounter > 0f)
        {
            bufferCounter = 0f;
            coyoteCounter = 0f;

            rb.linearVelocity = new Vector2(rb.linearVelocity.x, 0f);
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);

             if (anim) anim.SetTrigger("Jump");

            didJump = true;
        }

        // better jump feel (gravity multipliers)
        if (rb.linearVelocity.y < 0)
        {
            rb.linearVelocity += Vector2.up * Physics2D.gravity.y * (fallMultiplier - 1) * Time.deltaTime;
        }
        else if (rb.linearVelocity.y > 0 && !Input.GetButton("Jump"))
        {
            rb.linearVelocity += Vector2.up * Physics2D.gravity.y * (lowJumpMultiplier - 1) * Time.deltaTime;
        }

        // animator: Speed (sudah ada)
        float x = Input.GetAxisRaw("Horizontal");
        float spd = Mathf.Abs(x);
        if (spd < 0.01f) spd = 0f;
        if (anim) anim.SetFloat("Speed", spd);

        // animator: Jump/Fall params
        if (anim)
        {
            anim.SetBool("IsGrounded", isGrounded);
            anim.SetFloat("YVelocity", rb.linearVelocity.y);

            // opsional: trigger jump saat lompat (kalau kamu pakai Trigger di Animator)
            if (didJump)
            {
                anim.SetTrigger("Jump");
            }

            // opsional: kalau kamu punya anim Land pakai trigger
            // if (!wasGrounded && isGrounded) anim.SetTrigger("Land");
        }
    }

    void FixedUpdate()
    {
        float x = Input.GetAxisRaw("Horizontal");
        rb.linearVelocity = new Vector2(x * speed, rb.linearVelocity.y);

        // flip
        if (x != 0)
        {
            Vector3 s = transform.localScale;
            s.x = Mathf.Abs(s.x) * (x > 0 ? 1 : -1);
            transform.localScale = s;
        }
    }

    void OnDrawGizmosSelected()
    {
        if (!groundCheck) return;
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(groundCheck.position, groundRadius);
    }
}
