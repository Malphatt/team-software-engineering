using System.Collections;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public GameObject Player;
    [SerializeField] EnemyData EnemyData;
    [SerializeField] ParticleSystem sparks;
    [SerializeField] int particleCount;
    public float health;
    public bool dead = false;

    void Start()
    {
        health = EnemyData.Health;
    }

    public void TakeDamage(float damage)
    {
        transform.GetComponent<ParticleSystem>().Emit(particleCount * JuiceSlider.Instance.juiciness);
        health -= damage;
        if (health <= 0f)
        {
            Destroy(gameObject, EnemyData.DeathDelay);
            dead = true;
        }
    }
}
