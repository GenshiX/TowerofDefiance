using UnityEngine;

[CreateAssetMenu(menuName = "Idle Tower/Tower Level Data")]
public class TowerLevelData : ScriptableObject
{
    public string displayName;
    public string materialTheme;

    [Header("Content")]
    public UnitData[] availableUnits;
    public EnemyData[] enemyRotation;

    [Header("Unlocking")]
    public bool unlockedByDefault;
    public int unlockCost;
}
