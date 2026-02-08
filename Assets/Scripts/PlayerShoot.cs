using UnityEngine;

public class PlayerShoot : MonoBehaviour
{
    public GameObject projectilePrefab;
    public Transform firePoint;

    private Animator anim;
    private bool isAttacking;

    void Awake()
    {
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && !isAttacking)
        {
            isAttacking = true;
            if (anim != null) anim.SetTrigger("Attack");
        }
    }

    // DIPANGGIL DARI ANIMATION EVENT
    public void SpawnFireball()
    {
        if (projectilePrefab == null || firePoint == null)
        {
            Debug.LogError("PlayerShoot: projectilePrefab atau firePoint belum di-assign!");
            return;
        }

        GameObject obj = Instantiate(projectilePrefab, firePoint.position, Quaternion.identity);

        Vector2 dir = transform.localScale.x >= 0 ? Vector2.right : Vector2.left;

        var proj = obj.GetComponent<FireProjectile>();
        if (proj != null) proj.Init(dir);
    }

    // DIPANGGIL DARI ANIMATION EVENT (di frame terakhir)
    public void EndAttack()
    {
        isAttacking = false;
    }
}
