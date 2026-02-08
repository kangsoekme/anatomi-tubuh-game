using UnityEngine;

public class HiddenBlockTrigger : MonoBehaviour
{
    public HiddenBlock block;
    public string playerTag = "Player";

    void Reset()
    {
        block = GetComponentInParent<HiddenBlock>();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag(playerTag)) return;

        // Pastikan pemain sedang bergerak ke atas (biar benar-benar "nyenggol dari bawah")
        Rigidbody2D prb = other.attachedRigidbody;
        if (prb != null && prb.linearVelocity.y <= 0f) return;

        block.ActivateFromBelow(other.gameObject);
    }
}
