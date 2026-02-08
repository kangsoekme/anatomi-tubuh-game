using UnityEngine;

public class ResetPrefsOnce : MonoBehaviour
{
    void Start()
    {
        PlayerPrefs.DeleteAll();
        PlayerPrefs.Save();
        Debug.Log("PlayerPrefs cleared!");
        Destroy(gameObject);
    }
}
