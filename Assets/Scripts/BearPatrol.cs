using UnityEngine;

public class BearPatrol : MonoBehaviour
{
    public float speed = 2f;
    public Transform leftPoint;
    public Transform rightPoint;

    private Rigidbody2D rb;
    private bool movingRight = true;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate()
    {
        float dir = movingRight ? 1f : -1f;
        rb.linearVelocity = new Vector2(dir * speed, rb.linearVelocity.y);

        if (movingRight && transform.position.x >= rightPoint.position.x)
            Flip(false);
        else if (!movingRight && transform.position.x <= leftPoint.position.x)
            Flip(true);
    }

    void Flip(bool goRight)
    {
        movingRight = goRight;
        Vector3 s = transform.localScale;
        s.x = Mathf.Abs(s.x) * (movingRight ? 1f : -1f);
        transform.localScale = s;
    }
}
