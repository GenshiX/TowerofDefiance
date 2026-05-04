using UnityEngine;

[CreateAssetMenu(menuName = "Idle Tower/Enemy Data")]
public class EnemyData : ScriptableObject
{
    [Header("Identity")]
    public string displayName;
    public TowerLevelData level;

    [Header("Stats")]
    public float baseHealth = 10f;
    public int goldReward = 1;

    [Header("Progression")]
    public float healthGrowthPerSpawn = 0.15f;
    public float rewardGrowthPerSpawn = 0.10f;
}
