using UnityEngine;

public class FallingTilemap2D : MonoBehaviour
{
    public float fallDelay = 0f;

    Rigidbody2D rb;
    bool triggered;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        if (rb == null) Debug.LogError("FallingTilemap2D: Rigidbody2D tidak ada!");

        rb.bodyType = RigidbodyType2D.Kinematic;
        rb.gravityScale = 1f;
        rb.freezeRotation = true;

        rb.interpolation = RigidbodyInterpolation2D.Interpolate;
        rb.collisionDetectionMode = CollisionDetectionMode2D.Continuous;
    }

    public void TriggerFall()
    {
        if (triggered) return;
        triggered = true;

        if (fallDelay <= 0f)
        {
            StartFallNow();
        }
        else
        {
            Invoke(nameof(StartFallNow), fallDelay);
        }
    }

    void StartFallNow()
    {
        rb.bodyType = RigidbodyType2D.Dynamic;

        // kasih dorongan kecil supaya langsung “pecah” dari kontak statis
        rb.linearVelocity = new Vector2(0f, -0.2f);
        rb.WakeUp();
    }
}
