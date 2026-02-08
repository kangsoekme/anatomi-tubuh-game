using System.Collections;
using UnityEngine;

public class HiddenBlock : MonoBehaviour
{
    [Header("References")]
    public SpriteRenderer sr;
    public Collider2D solidCollider;     // BoxCollider2D yang jadi ground
    public Collider2D hitTrigger;        // Trigger di bawah blok (child)

    [Header("Bump Settings")]
    public float bumpHeight = 0.15f;
    public float bumpTime = 0.08f;

    bool activated = false;
    Vector3 startPos;
    Rigidbody2D rb;

    void Reset()
    {
        sr = GetComponent<SpriteRenderer>();
        solidCollider = GetComponent<Collider2D>();
        rb = GetComponent<Rigidbody2D>();
    }

    void Awake()
    {
        startPos = transform.position;
        rb = GetComponent<Rigidbody2D>();

        // Mulai hidden + tidak solid
        if (sr) sr.enabled = false;
        if (solidCollider) solidCollider.enabled = false;

        // Anti jatuh (kalau ada Rigidbody2D)
        if (rb)
        {
            rb.bodyType = RigidbodyType2D.Kinematic;
            rb.gravityScale = 0f;
            rb.freezeRotation = true;
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        // Script ini akan menangkap trigger kalau trigger collider ada di object yang sama.
        // Kalau trigger ada di child, pakai cara di step 6 (Forwarder) ATAU pindahkan trigger ke parent.
    }

    public void ActivateFromBelow(GameObject player)
    {
        if (activated) return;

        // Optional: validasi benar-benar kena dari bawah
        if (player.transform.position.y > transform.position.y) return;

        activated = true;

        if (sr) sr.enabled = true;                 // muncul
        if (solidCollider) solidCollider.enabled = true; // jadi solid

        StartCoroutine(Bump());
    }

    IEnumerator Bump()
    {
        Vector3 upPos = startPos + Vector3.up * bumpHeight;

        // naik
        float t = 0f;
        while (t < bumpTime)
        {
            t += Time.deltaTime;
            transform.position = Vector3.Lerp(startPos, upPos, t / bumpTime);
            yield return null;
        }

        // turun
        t = 0f;
        while (t < bumpTime)
        {
            t += Time.deltaTime;
            transform.position = Vector3.Lerp(upPos, startPos, t / bumpTime);
            yield return null;
        }

        transform.position = startPos;
    }
}
