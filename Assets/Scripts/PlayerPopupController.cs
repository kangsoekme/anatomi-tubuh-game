using System.Collections;
using TMPro;
using UnityEngine;

public class PaperPopupController : MonoBehaviour
{
    [Header("UI Refs")]
    public RectTransform panel;
    public TMP_Text messageText;

    [Header("Typing")]
    public float typingSpeed = 0.03f;

    [Header("Timing")]
    public float visibleDuration = 7f;     // total durasi tampil (sesudah semua typing selesai)
    public float gapBetweenMessages = 0.35f;

    [Header("Slide Animation")]
    public float slideDuration = 0.6f;
    public float shownY = 80f;
    public float hiddenY = -400f;

    Coroutine routine;

    void Awake()
    {
        panel.anchoredPosition = new Vector2(0f, hiddenY);
        if (messageText != null) messageText.text = "";
    }

    // INI yang kita pakai untuk 2 pesan (atau lebih)
    public void ShowSequence(params string[] messages)
    {
        if (routine != null) StopCoroutine(routine);
        routine = StartCoroutine(ShowSequenceRoutine(messages));
    }

    IEnumerator ShowSequenceRoutine(string[] messages)
    {
        // slide in sekali
        yield return Slide(panel, hiddenY, shownY, slideDuration);

        // tampilkan semua pesan bergantian di popup yang sama
        for (int i = 0; i < messages.Length; i++)
        {
            // typing message i
            yield return TypeText(messages[i]);

            // jeda sebelum ganti ke message berikutnya
            if (i < messages.Length - 1)
                yield return new WaitForSeconds(gapBetweenMessages);
        }

        // stay (7 detik) setelah semua pesan selesai ditampilkan
        yield return new WaitForSeconds(visibleDuration);

        // slide out sekali
        yield return Slide(panel, shownY, hiddenY, slideDuration);

        messageText.text = "";
        routine = null;
    }

    IEnumerator TypeText(string fullText)
    {
        messageText.text = "";
        for (int i = 0; i < fullText.Length; i++)
        {
            messageText.text += fullText[i];
            yield return new WaitForSeconds(typingSpeed);
        }
    }

    IEnumerator Slide(RectTransform rt, float fromY, float toY, float duration)
    {
        float t = 0f;
        while (t < duration)
        {
            t += Time.unscaledDeltaTime;
            float x = Mathf.Clamp01(t / duration);
            float eased = EaseInOutCubic(x);

            float y = Mathf.Lerp(fromY, toY, eased);
            rt.anchoredPosition = new Vector2(0f, y);
            yield return null;
        }
        rt.anchoredPosition = new Vector2(0f, toY);
    }

    float EaseInOutCubic(float x)
    {
        return x < 0.5f ? 4f * x * x * x : 1f - Mathf.Pow(-2f * x + 2f, 3f) / 2f;
    }
}
