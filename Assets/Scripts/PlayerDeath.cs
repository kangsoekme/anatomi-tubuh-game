using UnityEngine;

public class PlayerDeath : MonoBehaviour
{
    public string fireTag = "Fire";
    public string enemyTag = "Enemies";

    public PlayerDeathSequence deathSequence;

    float invincibleTimer = 0f;
    bool isDead = false;

    void Awake()
    {
        if (deathSequence == null)
            deathSequence = GetComponent<PlayerDeathSequence>();
    }

    void Update()
    {
        if (invincibleTimer > 0f) invincibleTimer -= Time.deltaTime;
    }

    public void SetInvincible(float sec) => invincibleTimer = sec;
    public void ResetDeadFlag() => isDead = false;

    public void Die()
    {
        if (isDead) return;
        isDead = true;

        if (deathSequence != null) deathSequence.Die();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (invincibleTimer > 0f) return;
        if (isDead) return;

        if (other.CompareTag(fireTag) || other.CompareTag(enemyTag))
            Die();
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (invincibleTimer > 0f) return;
        if (isDead) return;

        if (collision.collider.CompareTag(fireTag) || collision.collider.CompareTag(enemyTag))
            Die();
    }
}
