using UnityEngine;

[CreateAssetMenu(menuName = "Idle Tower/Upgrade Data")]
public class UpgradeData : ScriptableObject
{
    [Header("Identity")]
    public string displayName;
    public UnitData targetUnit;

    [Header("Effect")]
    public UpgradeEffectType effectType;
    public int maxLevel = 10;
    public float valuePerLevel = 0.10f;

    [Header("Cost")]
    public int baseCost = 10;
    public float costMultiplier = 1.15f;

    [Header("Optional Text")]
    [TextArea]
    public string description;
    public string finalLevelName;

    public int GetCostForNextLevel(int currentLevel)
    {
        return Mathf.RoundToInt(baseCost * Mathf.Pow(costMultiplier, currentLevel));
    }

    public void Apply(ref UnitStats stats, int level)
    {
        float totalValue = valuePerLevel * level;

        switch (effectType)
        {
            case UpgradeEffectType.DamageMultiplier:
                stats.Damage *= 1f + totalValue;
                break;

            case UpgradeEffectType.FlatDamage:
                stats.Damage += totalValue;
                break;

            case UpgradeEffectType.AttackRateMultiplier:
            case UpgradeEffectType.ReloadSpeed:
                stats.AttackRate *= 1f + totalValue;
                break;

            case UpgradeEffectType.CooldownReduction:
                stats.AttackRate *= 1f + totalValue;
                break;

            case UpgradeEffectType.ProjectileCount:
                stats.ProjectileCount += Mathf.FloorToInt(totalValue);
                break;

            case UpgradeEffectType.ProjectileSize:
                stats.ProjectileSize *= 1f + totalValue;
                break;

            case UpgradeEffectType.ProjectileSpeed:
                stats.ProjectileSpeed *= 1f + totalValue;
                break;

            case UpgradeEffectType.BeamFocus:
                stats.BeamFocusMultiplier *= 1f + totalValue;
                break;

            case UpgradeEffectType.Accuracy:
            case UpgradeEffectType.CriticalChance:
            case UpgradeEffectType.SplashRadius:
            case UpgradeEffectType.Special:
                // TODO: Add these when those combat systems exist.
                break;
        }
    }
}
