using UnityEngine;

[CreateAssetMenu(fileName = "Enemy_Name", menuName = "EnemyData")]
public class EnemyData : ScriptableObject
{
    [Header("Enemy Info")]
    public string Name;
    public float Health;
    public float DeathDelay;
}
