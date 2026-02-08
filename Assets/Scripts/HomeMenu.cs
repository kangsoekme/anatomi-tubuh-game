using UnityEngine;
using UnityEngine.SceneManagement;

public class HomeMenu : MonoBehaviour
{
    public void PlayGame()
    {
        if (FadeTransition.Instance != null)
        {
            FadeTransition.Instance.FadeToScene("MainScene");
        }
        else
        {
            SceneManager.LoadScene("MainScene");
        }
    }

    public void QuitGame()
    {
        Application.Quit();
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}