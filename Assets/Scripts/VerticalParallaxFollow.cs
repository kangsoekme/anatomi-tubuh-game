using UnityEngine;

public class VerticalParallaxFollow : MonoBehaviour
{
    [Header("References")]
    public Transform target; // player atau camera

    [Header("Parallax Settings")]
    [Range(0f, 1f)]
    public float followRatio = 0.3f; // 0.3 = ikut 30% dari gerak target
    public float yOffset = 0f;
    public float smooth = 8f;

    float startY;
    float targetStartY;

    void Start()
    {
        startY = transform.position.y;
        if (target != null) targetStartY = target.position.y;
    }

    void LateUpdate()
    {
        if (target == null) return;

        // delta gerak target dari posisi awalnya
        float deltaY = target.position.y - targetStartY;

        // posisi awan = posisi awal + deltaY * ratio + offset
        float desiredY = startY + (deltaY * followRatio) + yOffset;

        Vector3 pos = transform.position;
        pos.y = Mathf.Lerp(pos.y, desiredY, Time.deltaTime * smooth);
        transform.position = pos;
    }
}
