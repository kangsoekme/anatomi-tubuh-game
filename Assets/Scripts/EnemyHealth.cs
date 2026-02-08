using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    public int hp = 1;

    public void TakeDamage(int dmg)
    {
        hp -= dmg;
        if (hp <= 0)
        {
            Destroy(gameObject);
        }
    }

    public void Die()
{
    Destroy(gameObject);
}
}
