using UnityEngine;
using UnityEngine.Tilemaps;

public class FireProjectile : MonoBehaviour
{
    public float speed = 8f;
    public float maxDistance = 3f; // 3 tile
    public int damage = 1;

    private Vector2 dir;
    private Vector3 startPos;

    public Transform visual;

    public void Init(Vector2 direction)
    {
        dir = direction.normalized;
        startPos = transform.position;

        // flip kiri / kanan
        if (visual != null)
        {
            Vector3 s = visual.localScale;
            s.x = dir.x < 0 ? -Mathf.Abs(s.x) : Mathf.Abs(s.x);
            visual.localScale = s;
        }
    }

    void Update()
    {
        transform.Translate(dir * speed * Time.deltaTime);

        if (Vector3.Distance(startPos, transform.position) >= maxDistance)
            Destroy(gameObject);
    }

private void OnTriggerEnter2D(Collider2D other)
{
    // 1) Cari EnemyHealth di parent (kalau kamu pakai sistem HP)
    EnemyHealth eh = other.GetComponentInParent<EnemyHealth>();
    if (eh != null)
    {
        // kalau mau langsung mati:
        Destroy(eh.gameObject);
        Destroy(gameObject);
        return;
    }

    // 2) Kalau tidak pakai EnemyHealth, tetap bunuh object bertag Enemies di parent
    if (other.CompareTag("Enemies") || other.transform.root.CompareTag("Enemies"))
    {
        GameObject enemyObj = other.CompareTag("Enemies") ? other.gameObject : other.transform.root.gameObject;
        Destroy(enemyObj);
        Destroy(gameObject);
        return;
    }

    // 3) Ground / tembok dll
    if (other.CompareTag("Ground"))
    {
        Destroy(gameObject);
    }
}
}
