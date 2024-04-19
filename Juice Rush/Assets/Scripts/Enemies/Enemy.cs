using UnityEngine;

public class Enemy : MonoBehaviour
{
    public GameObject Player;
    [SerializeField] EnemyData EnemyData;
    public float health;

    void Start()
    {
        health = EnemyData.Health;
    }

    public void TakeDamage(float damage)
    {
        health -= damage;
        if (health <= 0f)
            Destroy(gameObject, EnemyData.DeathDelay);
    }
}
