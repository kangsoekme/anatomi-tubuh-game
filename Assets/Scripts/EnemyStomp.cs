using UnityEngine;

public class EnemyStomp : MonoBehaviour
{
    [Header("Bounce")]
    public float bounceForce = 12f;

    [Header("Stomp Condition")]
    public float mustBeFallingBelow = -0.1f; // player harus sedang turun (velocity y negatif)

    private EnemyHealth enemyHealth;

    private void Awake()
    {
        enemyHealth = GetComponentInParent<EnemyHealth>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        Rigidbody2D prb = other.GetComponent<Rigidbody2D>();
        if (prb == null) return;

        // hanya stomp kalau player jatuh
        if (prb.linearVelocity.y < mustBeFallingBelow)
        {
            // bounce player setelah stomp
            prb.linearVelocity = new Vector2(prb.linearVelocity.x, bounceForce);

            // kill enemy
            if (enemyHealth != null) enemyHealth.Die();
            else Destroy(transform.root.gameObject);
        }
    }
}
