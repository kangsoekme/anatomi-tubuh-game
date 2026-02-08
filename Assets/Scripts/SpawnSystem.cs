using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SpawnSystem : MonoBehaviour
{
    public static SpawnSystem Instance { get; private set; }

    private Vector3 spawnPos = Vector3.zero;
    private bool hasCheckpoint = false; // <-- INI KUNCI

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        SceneManager.sceneLoaded += OnSceneLoaded;

        // cuma ambil spawn awal sekali
        FindInitialSpawnPointIfNeeded();
    }

    void OnDestroy()
    {
        if (Instance == this)
            SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // JANGAN overwrite checkpoint
        FindInitialSpawnPointIfNeeded();
    }

    void FindInitialSpawnPointIfNeeded()
    {
        // kalau sudah punya checkpoint, jangan diganti
        if (hasCheckpoint) return;

        var sp = GameObject.FindGameObjectWithTag("SpawnPoint");
        if (sp != null)
        {
            spawnPos = sp.transform.position;
            Debug.Log($"[SpawnSystem] Initial SpawnPoint: name={sp.name} pos={spawnPos}");
        }
        else
        {
            Debug.LogError("[SpawnSystem] Tidak menemukan object dengan tag 'SpawnPoint'!");
        }
    }

    public Vector3 GetSpawnPosition() => spawnPos;

    public void SetSpawnPosition(Vector3 newPos)
    {
        spawnPos = newPos;
        hasCheckpoint = true; // <-- INI KUNCI
        Debug.Log($"[SpawnSystem] Checkpoint updated to {spawnPos}");
    }

    public void ClearCheckpoint()
    {
        hasCheckpoint = false;
        FindInitialSpawnPointIfNeeded();
    }

    public void ReloadCurrentScene()
    {
        StartCoroutine(ReloadRoutine());
    }

    IEnumerator ReloadRoutine()
    {
        int buildIndex = SceneManager.GetActiveScene().buildIndex;

        Debug.Log("[SpawnSystem] Reloading scene...");
        var op = SceneManager.LoadSceneAsync(buildIndex);
        while (!op.isDone)
            yield return null;

        yield return null; // tunggu 1 frame

        var player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            player.transform.position = spawnPos;
            Physics2D.SyncTransforms();

            var rb = player.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                rb.linearVelocity = Vector2.zero;
                rb.angularVelocity = 0f;
                rb.position = spawnPos;
            }

            Debug.Log($"[SpawnSystem] Player moved to spawnPos={spawnPos}");
        }
        else
        {
            Debug.LogError("[SpawnSystem] Player tag 'Player' tidak ditemukan setelah reload!");
        }
    }
}
