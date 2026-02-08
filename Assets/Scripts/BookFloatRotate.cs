using UnityEngine;

public class BookFloatRotate : MonoBehaviour
{
    [Header("Floating")]
    public float floatAmplitude = 0.15f;
    public float floatFrequency = 2f;

    [Header("Rotation (disable if you don't want rotate)")]
    public bool enableRotate = false;   // <-- default MATI
    public float rotateSpeed = 120f;

    Vector3 startPos;
    Quaternion startRot;

    void Start()
    {
        startPos = transform.localPosition;
        startRot = transform.localRotation;
    }

    void Update()
    {
        // Ngambang naik-turun
        float y = Mathf.Sin(Time.time * floatFrequency) * floatAmplitude;
        transform.localPosition = startPos + new Vector3(0, y, 0);

        // Rotate hanya jika enableRotate = true
        if (enableRotate)
        {
            transform.Rotate(0, 0, rotateSpeed * Time.deltaTime);
        }
        else
        {
            // pastikan rotasi tetap seperti awal (tidak muter sama sekali)
            transform.localRotation = startRot;
        }
    }
}
