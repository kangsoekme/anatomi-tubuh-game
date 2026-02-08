using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class QuizPopupController : MonoBehaviour
{
    [Header("UI Refs (assign via Inspector)")]
    public RectTransform panel;       // Root panel yang di-slide
    public TMP_Text questionText;     // TMP untuk pertanyaan
    public Button btnA;
    public Button btnB;
    public Button btnC;

    [Header("Slide Settings")]
    public float slideDuration = 0.35f;
    public float shownX = -40f;
    public float hiddenX = 700f;

    private Action<bool> onAnswered;
    private bool isOpen;

    void Awake()
    {
        if (panel != null)
            panel.anchoredPosition = new Vector2(hiddenX, panel.anchoredPosition.y);

        gameObject.SetActive(false);
    }

    /// <summary>
    /// correctIndex: 0 = A, 1 = B, 2 = C
    /// option sprites = gambar tombol jawaban (karena button pakai Image)
    /// </summary>
    public void Open(string question, Sprite optionA, Sprite optionB, Sprite optionC, int correctIndex, Action<bool> callback)
    {
        if (panel == null || questionText == null || btnA == null || btnB == null || btnC == null)
        {
            Debug.LogError("QuizPopupController: panel/questionText/btnA/btnB/btnC belum di-assign di Inspector.");
            return;
        }

        if (isOpen) return;
        isOpen = true;

        onAnswered = callback;

        // Set pertanyaan
        questionText.text = question;

        // Set sprite untuk tombol (karena kamu pakai Image)
        ApplyButtonSprite(btnA, optionA);
        ApplyButtonSprite(btnB, optionB);
        ApplyButtonSprite(btnC, optionC);

        // Reset listener
        btnA.onClick.RemoveAllListeners();
        btnB.onClick.RemoveAllListeners();
        btnC.onClick.RemoveAllListeners();

        btnA.onClick.AddListener(() => Answer(0, correctIndex));
        btnB.onClick.AddListener(() => Answer(1, correctIndex));
        btnC.onClick.AddListener(() => Answer(2, correctIndex));

        gameObject.SetActive(true);
        StopAllCoroutines();
        StartCoroutine(Slide(hiddenX, shownX));
    }

    private void ApplyButtonSprite(Button btn, Sprite sprite)
    {
        if (btn == null) return;
        if (sprite == null) return; // kalau null, biarkan sprite default dari inspector

        var img = btn.image; // Image bawaan Button
        if (img != null)
            img.sprite = sprite;
    }

    private void Answer(int pickedIndex, int correctIndex)
    {
        bool correct = pickedIndex == correctIndex;
        StopAllCoroutines();
        StartCoroutine(CloseThenCallback(correct));
    }

    private IEnumerator CloseThenCallback(bool correct)
    {
        yield return Slide(shownX, hiddenX);
        gameObject.SetActive(false);

        isOpen = false;

        onAnswered?.Invoke(correct);
        onAnswered = null;
    }

    private IEnumerator Slide(float fromX, float toX)
    {
        float t = 0f;
        Vector2 start = new Vector2(fromX, panel.anchoredPosition.y);
        Vector2 end = new Vector2(toX, panel.anchoredPosition.y);

        while (t < slideDuration)
        {
            t += Time.unscaledDeltaTime;
            float x = Mathf.Clamp01(t / slideDuration);

            float eased = x < 0.5f ? 4f * x * x * x : 1f - Mathf.Pow(-2f * x + 2f, 3f) / 2f;

            panel.anchoredPosition = Vector2.Lerp(start, end, eased);
            yield return null;
        }

        panel.anchoredPosition = end;
    }
}
