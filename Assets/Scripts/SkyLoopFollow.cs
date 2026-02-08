using UnityEngine;

public class SkyLoopFollow : MonoBehaviour
{
    [Header("References")]
    public Transform cam;        // Main Camera (Cinemachine menggerakkan ini)
    public Transform player;

    [Header("Tiles (parent objects)")]
    public Transform[] tiles;    // isi 3: kiri, tengah, kanan

    [Header("Loop Settings")]
    public float recycleBuffer = 2f;     // makin besar = recycle lebih cepat (lebih aman)
    public float seamEpsilon = 0.01f;    // biar tidak ada garis tipis gap

    [Header("Vertical Follow")]
    [Range(0f, 1f)] public float verticalFollowStrength = 1f;
    public float yOffset = 0f;
    public float ySmooth = 5f;

    void Awake()
    {
        if (cam == null) cam = Camera.main != null ? Camera.main.transform : null;

        if (cam == null)
        {
            Debug.LogError("Camera belum di-assign dan Camera.main tidak ditemukan.");
            enabled = false;
            return;
        }

        if (tiles == null || tiles.Length < 3 || tiles[0] == null || tiles[1] == null || tiles[2] == null)
        {
            Debug.LogError("Assign tiles (3 parent objects) dulu (kiri, tengah, kanan).");
            enabled = false;
            return;
        }
    }

    void LateUpdate()
    {
        HandleVerticalFollow();
        HandleHorizontalLoop();
    }

    void HandleVerticalFollow()
    {
        if (player == null) return;

        // >>> FIX: kalau player sedang mati/dying, jangan ikut turun
        var ds = player.GetComponent<PlayerDeathSequence>();
        if (ds != null && ds.IsDying) return;

        float targetY = Mathf.Lerp(transform.position.y, player.position.y, verticalFollowStrength) + yOffset;
        float newY = Mathf.Lerp(transform.position.y, targetY, 1f - Mathf.Exp(-ySmooth * Time.deltaTime));
        transform.position = new Vector3(transform.position.x, newY, transform.position.z);
    }

    void HandleHorizontalLoop()
    {
        float camX = cam.position.x;

        Transform left = tiles[0];
        Transform mid  = tiles[1];
        Transform right= tiles[2];

        Bounds bLeft  = GetTileBounds(left);
        Bounds bMid   = GetTileBounds(mid);
        Bounds bRight = GetTileBounds(right);

        // Kalau kamera hampir melewati ujung kanan tile mid -> recycle tile kiri ke kanan
        if (camX > bMid.max.x - recycleBuffer)
        {
            float targetLeftEdge = bRight.max.x - seamEpsilon;
            ShiftTileToLeftEdge(left, targetLeftEdge);

            tiles[0] = mid;
            tiles[1] = right;
            tiles[2] = left;
        }
        // Kalau kamera hampir melewati ujung kiri tile mid -> recycle tile kanan ke kiri
        else if (camX < bMid.min.x + recycleBuffer)
        {
            float targetRightEdge = bLeft.min.x + seamEpsilon;
            ShiftTileToRightEdge(right, targetRightEdge);

            tiles[2] = mid;
            tiles[1] = left;
            tiles[0] = right;
        }
    }

    Bounds GetTileBounds(Transform tileRoot)
    {
        var renderers = tileRoot.GetComponentsInChildren<SpriteRenderer>();
        if (renderers == null || renderers.Length == 0)
        {
            return new Bounds(tileRoot.position, Vector3.one);
        }

        Bounds b = renderers[0].bounds;
        for (int i = 1; i < renderers.Length; i++)
            b.Encapsulate(renderers[i].bounds);

        return b;
    }

    void ShiftTileToLeftEdge(Transform tile, float targetLeftEdgeX)
    {
        Bounds b = GetTileBounds(tile);
        float delta = targetLeftEdgeX - b.min.x;
        tile.position += new Vector3(delta, 0f, 0f);
    }

    void ShiftTileToRightEdge(Transform tile, float targetRightEdgeX)
    {
        Bounds b = GetTileBounds(tile);
        float delta = targetRightEdgeX - b.max.x;
        tile.position += new Vector3(delta, 0f, 0f);
    }
}
