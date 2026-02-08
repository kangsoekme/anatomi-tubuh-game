using UnityEngine;
using UnityEngine.SceneManagement;

public class PopupAutoStart : MonoBehaviour
{
    public GameObject popup;
    public string menuSceneName = "HomeScene";
    public float delay = 0.4f;

    [Header("Startup Messages")]
    [TextArea] public string message1 = "Selamat datang!";
    [TextArea] public string message2 = "Hati-hati jebakan 😄";

    // ✅ hanya sekali per sesi play
    private static bool shownThisSession = false;

    void OnEnable()
    {
        // jangan jalankan di HomeScene
        if (SceneManager.GetActiveScene().name == menuSceneName) return;
        if (popup == null) return;

        // ✅ kalau sudah pernah tampil, jangan tampil lagi
        if (shownThisSession) return;

        popup.SetActive(false);
        CancelInvoke();
        Invoke(nameof(Show), delay);
    }

    void Show()
    {
        if (popup == null) return;

        shownThisSession = true;

        popup.SetActive(true);

        var controller = popup.GetComponent<PaperPopupController>();
        if (controller != null)
        {
            controller.ShowSequence(message1, message2);
        }
        else
        {
            Debug.LogWarning("[PopupAutoStart] PaperPopupController tidak ditemukan di object popup.");
        }
    }
}
