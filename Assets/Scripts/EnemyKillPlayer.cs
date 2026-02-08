using UnityEngine;

public class EnemyKillPlayer : MonoBehaviour
{
    [Header("Stomp Ignore")]
    public float stompYOffset = 0.05f;   // toleransi posisi
    public float mustBeFallingBelow = -0.1f; // harus turun untuk dianggap stomp

    private Collider2D myCol;

    void Awake()
    {
        myCol = GetComponent<Collider2D>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        Rigidbody2D prb = other.GetComponent<Rigidbody2D>();
        if (prb != null)
        {
            // Jika player jatuh dari atas, JANGAN bunuh player (biarkan HeadTrigger yang handle)
            float playerBottom = other.bounds.min.y;
            float enemyTop = myCol.bounds.max.y;

            bool playerFromAbove = playerBottom >= (enemyTop - stompYOffset);
            bool playerFalling = prb.linearVelocity.y < mustBeFallingBelow;

            if (playerFromAbove && playerFalling)
            {
                return; // ignore: ini stomp
            }
        }

        // Kalau bukan stomp → baru bunuh player
        // Panggil method kamu yang existing (reload scene / reduce life / dll)
        // Contoh (sesuaikan dengan projectmu):
        // other.GetComponent<PlayerDeath>()?.Die();

        PlayerDeath pd = other.GetComponent<PlayerDeath>();
        if (pd != null) pd.Die();   // <- kalau methodnya beda, ganti sesuai punyamu
    }
}
