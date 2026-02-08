using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class FadeTransition : MonoBehaviour
{
    public static FadeTransition Instance { get; private set; }

    [Header("Fade Settings")]
    [SerializeField] private float fadeDuration = 1f;
    [SerializeField] private Color fadeColor = Color.black;

    private Image fadeImage;
    private Canvas fadeCanvas;
    private bool isFading = false;

    void Awake()
    {
        // Singleton pattern
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        SetupFadeCanvas();
    }

    void SetupFadeCanvas()
    {
        // Create Canvas
        GameObject canvasObj = new GameObject("FadeCanvas");
        canvasObj.transform.SetParent(transform);

        fadeCanvas = canvasObj.AddComponent<Canvas>();
        fadeCanvas.renderMode = RenderMode.ScreenSpaceOverlay;
        fadeCanvas.sortingOrder = 9999;

        CanvasScaler scaler = canvasObj.AddComponent<CanvasScaler>();
        scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
        scaler.referenceResolution = new Vector2(1920, 1080);

        canvasObj.AddComponent<GraphicRaycaster>();

        // Create Fade Image
        GameObject imageObj = new GameObject("FadeImage");
        imageObj.transform.SetParent(canvasObj.transform, false);

        fadeImage = imageObj.AddComponent<Image>();
        fadeImage.color = new Color(fadeColor.r, fadeColor.g, fadeColor.b, 0f);

        RectTransform rt = fadeImage.GetComponent<RectTransform>();
        rt.anchorMin = Vector2.zero;
        rt.anchorMax = Vector2.one;
        rt.sizeDelta = Vector2.zero;
        rt.anchoredPosition = Vector2.zero;

        fadeCanvas.gameObject.SetActive(false);
    }

    // FADE OUT (hitam muncul)
    public void FadeOut(float duration = -1f)
    {
        if (isFading) return;
        if (duration < 0) duration = fadeDuration;
        StartCoroutine(FadeCoroutine(0f, 1f, duration));
    }

    // FADE IN (hitam hilang)
    public void FadeIn(float duration = -1f)
    {
        if (isFading) return;
        if (duration < 0) duration = fadeDuration;
        StartCoroutine(FadeCoroutine(1f, 0f, duration));
    }

    // FADE TO SCENE
    public void FadeToScene(string sceneName, float duration = -1f)
    {
        if (isFading) return;
        if (duration < 0) duration = fadeDuration;
        StartCoroutine(FadeToSceneCoroutine(sceneName, duration));
    }

    private IEnumerator FadeCoroutine(float startAlpha, float endAlpha, float duration)
    {
        isFading = true;
        fadeCanvas.gameObject.SetActive(true);

        float elapsed = 0f;
        Color color = fadeImage.color;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float alpha = Mathf.Lerp(startAlpha, endAlpha, elapsed / duration);
            fadeImage.color = new Color(color.r, color.g, color.b, alpha);
            yield return null;
        }

        fadeImage.color = new Color(color.r, color.g, color.b, endAlpha);

        if (endAlpha == 0f)
        {
            fadeCanvas.gameObject.SetActive(false);
        }
        
        isFading = false;
    }

    private IEnumerator FadeToSceneCoroutine(string sceneName, float duration)
    {
        yield return StartCoroutine(FadeCoroutine(0f, 1f, duration));
        SceneManager.LoadScene(sceneName);
        yield return StartCoroutine(FadeCoroutine(1f, 0f, duration));
    }
}