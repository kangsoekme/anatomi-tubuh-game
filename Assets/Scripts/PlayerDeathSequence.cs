using UnityEngine;
using System.Collections;

public class PlayerDeathSequence : MonoBehaviour
{
    [Header("Death Settings")]
    public float deathDelay = 1f;
    public float respawnDelay = 0.5f;

    [Header("References")]
    public Animator animator;
    public Rigidbody2D rb;
    public PlayerDeath playerDeath;

    private Player playerController;

    void Awake()
    {
        if (animator == null) animator = GetComponent<Animator>();
        if (rb == null) rb = GetComponent<Rigidbody2D>();
        if (playerDeath == null) playerDeath = GetComponent<PlayerDeath>();
        
        playerController = GetComponent<Player>();
    }

    public void Die()
    {
        StartCoroutine(DeathSequence());
    }

    private IEnumerator DeathSequence()
    {
        // Disable player control
        if (playerController != null) playerController.enabled = false;

        // Play death animation
        if (animator != null)
        {
            animator.SetTrigger("die");
        }

        // Freeze player
        if (rb != null)
        {
            rb.velocity = Vector2.zero;
            rb.isKinematic = true;
        }

        yield return new WaitForSeconds(deathDelay);

        // Fade out
        if (FadeTransition.Instance != null)
        {
            yield return StartCoroutine(FadeTransition.Instance.FadeOut());
        }

        // Respawn
        Respawn();

        yield return new WaitForSeconds(respawnDelay);

        // Fade in
        if (FadeTransition.Instance != null)
        {
            yield return StartCoroutine(FadeTransition.Instance.FadeIn());
        }
    }

    private void Respawn()
    {
        // Reset position
        if (SpawnSystem.Instance != null)
        {
            transform.position = SpawnSystem.Instance.GetSpawnPosition();
        }

        // Reset physics
        if (rb != null)
        {
            rb.isKinematic = false;
            rb.velocity = Vector2.zero;
        }

        // Reset animator
        if (animator != null)
        {
            animator.SetTrigger("idle");
        }

        // Re-enable controls
        if (playerController != null) playerController.enabled = true;

        // Reset death flag
        if (playerDeath != null)
        {
            playerDeath.ResetDeadFlag();
            playerDeath.SetInvincible(2f); // 2 detik invincible setelah respawn
        }
    }
}