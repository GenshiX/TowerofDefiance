using UnityEngine;

/// <summary>
/// Final calculated combat stats after base UnitData and upgrades are combined.
/// </summary>
[System.Serializable]
public struct UnitStats
{
    public float Damage;
    public float AttackRate;
    public float ProjectileSpeed;
    public int ProjectileCount;
    public float ProjectileSize;
    public float BeamFocusMultiplier;

    public static UnitStats From(UnitData unitData, UnitUpgradeState upgradeState)
    {
        UnitStats stats = new UnitStats
        {
            Damage = unitData != null ? unitData.baseDamage : 1f,
            AttackRate = unitData != null ? unitData.baseAttackRate : 1f,
            ProjectileSpeed = unitData != null ? unitData.baseProjectileSpeed : 8f,
            ProjectileCount = unitData != null ? unitData.baseProjectileCount : 1,
            ProjectileSize = unitData != null ? unitData.baseProjectileSize : 1f,
            BeamFocusMultiplier = unitData != null ? unitData.baseBeamFocusMultiplier : 1f
        };

        if (upgradeState != null)
            upgradeState.ApplyTo(ref stats);

        stats.AttackRate = Mathf.Max(0.01f, stats.AttackRate);
        stats.ProjectileCount = Mathf.Max(1, stats.ProjectileCount);
        stats.ProjectileSize = Mathf.Max(0.01f, stats.ProjectileSize);

        return stats;
    }
}
