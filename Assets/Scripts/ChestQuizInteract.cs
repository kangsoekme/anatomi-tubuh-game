using UnityEngine;
using UnityEngine.SceneManagement;
using Cainos.PixelArtPlatformer_VillageProps;
using System.Collections;

public class ChestQuizInteract : MonoBehaviour
{
    [Header("Book Requirement")]
    public string requiredBookId = "book_ginjal"; // Book yang harus diklaim dulu
    public bool requireBook = true;

    [Header("Refs (assign via inspector)")]
    public Chest chest;
    public GameObject quizPopup;
    public GameObject spikeBallPrefab;

    [Header("Checkpoint / Respawn")]
    public Transform checkpointPoint;
    public bool setCheckpointOnCorrect = true;

    [Header("SAVE (optional)")]
    public string chestId = "CHEST_01";
    public bool useSaveSystem = true;

    [Header("Interaction")]
    public Collider2D interactCollider;
    public string playerTag = "Player";
    public bool disableColliderWhenOpened = true;

    [Header("SpikeBall Drop")]
    public Transform spikeSpawnPoint;
    public Vector2 spikeSpawnOffset = new Vector2(0f, 3f);
    public int spikeCount = 1;
    public float spikeSpreadX = 0.6f;

    private bool playerInRange;
    private bool quizOpened;

    private string SaveKey =>
        $"CHEST_OPENED_{SceneManager.GetActiveScene().name}_{chestId}";

    private void Reset()
    {
        if (interactCollider == null) interactCollider = GetComponent<Collider2D>();
        if (chest == null) chest = GetComponent<Chest>();
    }

    private void Start()
    {
        // Cek apakah book sudah diklaim
        if (requireBook && BookManager.Instance != null)
        {
            if (!BookManager.Instance.IsBookClaimed(requiredBookId))
            {
                // Book belum diklaim, sembunyikan chest
                gameObject.SetActive(false);
                return;
            }
        }

        if (useSaveSystem && IsOpened())
            ApplyOpenedState();
        else
            ForceClosedState();
    }

    private void ForceClosedState()
    {
        if (quizPopup != null) quizPopup.SetActive(false);
        quizOpened = false;
    }

    private bool IsOpened()
    {
        return PlayerPrefs.GetInt(SaveKey, 0) == 1;
    }

    private void SetOpened()
    {
        PlayerPrefs.SetInt(SaveKey, 1);
        PlayerPrefs.Save();
    }

    private void ApplyOpenedState()
    {
        if (chest != null) chest.Open();

        if (disableColliderWhenOpened && interactCollider != null)
            interactCollider.enabled = false;

        if (quizPopup != null)
            quizPopup.SetActive(false);

        quizOpened = false;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag(playerTag)) return;

        if (useSaveSystem && IsOpened()) return;

        playerInRange = true;

        if (!quizOpened)
            OpenQuiz();
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (!other.CompareTag(playerTag)) return;

        playerInRange = false;

        if (quizPopup != null) quizPopup.SetActive(false);
        quizOpened = false;
    }

    private void OpenQuiz()
    {
        if (quizPopup == null) return;

        quizPopup.SetActive(true);
        quizOpened = true;
    }

    public void OnQuizCorrect()
    {
        if (setCheckpointOnCorrect && SpawnSystem.Instance != null)
        {
            Vector3 cp = checkpointPoint != null ? checkpointPoint.position : transform.position;
            SpawnSystem.Instance.SetSpawnPosition(cp);
        }

        if (useSaveSystem)
            SetOpened();

        ApplyOpenedState();
    }

    public void OnQuizWrong()
    {
        if (quizPopup != null)
            quizPopup.SetActive(false);

        quizOpened = false;

        DropSpikeBall();
    }

    private void DropSpikeBall()
    {
        if (spikeBallPrefab == null) return;

        Vector3 basePos;
        if (spikeSpawnPoint != null) basePos = spikeSpawnPoint.position;
        else basePos = transform.position + (Vector3)spikeSpawnOffset;

        for (int i = 0; i < spikeCount; i++)
        {
            float x = (spikeCount <= 1) ? 0f : Random.Range(-spikeSpreadX, spikeSpreadX);
            Vector3 pos = basePos + new Vector3(x, 0f, 0f);
            Instantiate(spikeBallPrefab, pos, Quaternion.identity);
        }
    }
}