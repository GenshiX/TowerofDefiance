using UnityEngine;

[CreateAssetMenu(menuName = "Idle Tower/Unit Data")]
public class UnitData : ScriptableObject
{
    [Header("Identity")]
    public string displayName;
    public TowerLevelData level;

    [Header("Base Stats")]
    public float baseDamage = 1f;
    public float baseAttackRate = 1f;
    public float baseProjectileSpeed = 8f;
    public int baseProjectileCount = 1;
    public float baseProjectileSize = 1f;
    public float baseBeamFocusMultiplier = 1f;

    [Header("Upgrades")]
    public UpgradeData[] upgrades;
}
