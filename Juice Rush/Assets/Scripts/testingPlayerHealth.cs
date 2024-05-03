using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class testingPlayerHealth : MonoBehaviour
{
    [SerializeField] public float health = 100f;
    [SerializeField] float healthRegenPts;
    [SerializeField] float healthRegenCooldown;
    float healthRegenTimer;
    private bool isDamageTaken;

    private void Update()
    {
        HealthRegeneration();
        if(health < 0)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }
    public void TakeDamage(float damagePoints)
    {
        health -= damagePoints;
        isDamageTaken = true;
        healthRegenTimer = healthRegenCooldown;
        if (health <= 0f)
        {
            Debug.Log("hit player");
        }
    }

    private void HealthRegeneration()
    {
        if (isDamageTaken)
        {
            healthRegenTimer -= Time.deltaTime;
        }

        if (healthRegenTimer <= 0f && isDamageTaken)
        {
            health += healthRegenPts * Time.deltaTime;
            if (health >= 100f)
            {
                health = 100f;
                isDamageTaken = false;
            }
        }
    }

}
