using UnityEngine;

public class MovingPlatform2D : MonoBehaviour
{
    public Transform pointA;
    public Transform pointB;
    public float speed = 2f;

    [Header("Variation")]
    public bool startGoingToB = true;

    private Vector3 _target;

    void Start()
    {
        if (pointA == null || pointB == null)
        {
            Debug.LogError("PointA / PointB belum di-assign!");
            enabled = false;
            return;
        }

        _target = startGoingToB ? pointB.position : pointA.position;
    }

    void FixedUpdate()
    {
        transform.position = Vector3.MoveTowards(transform.position, _target, speed * Time.fixedDeltaTime);

        if (Vector3.Distance(transform.position, _target) < 0.01f)
        {
            _target = (_target == pointB.position) ? pointA.position : pointB.position;
        }
    }
}
