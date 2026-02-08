using UnityEngine;

public class FallingTilemapTrigger : MonoBehaviour
{
    public FallingTilemap2D falling;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;
        if (falling == null) return;

        falling.TriggerFall();
    }
}
