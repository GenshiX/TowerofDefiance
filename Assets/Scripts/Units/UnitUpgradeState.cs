using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Runtime upgrade levels for one unit instance.
/// This intentionally stores levels separately from UpgradeData assets.
/// Data says what an upgrade does. State says how much the player bought.
/// </summary>
public class UnitUpgradeState : MonoBehaviour
{
    [SerializeField] private UnitData unitData;

    private readonly Dictionary<UpgradeData, int> levels = new Dictionary<UpgradeData, int>();

    private void Awake()
    {
        Initialize(unitData);
    }

    public void Initialize(UnitData data)
    {
        unitData = data;
        levels.Clear();

        if (unitData == null || unitData.upgrades == null)
            return;

        foreach (UpgradeData upgrade in unitData.upgrades)
        {
            if (upgrade != null && !levels.ContainsKey(upgrade))
                levels.Add(upgrade, 0);
        }
    }

    public int GetLevel(UpgradeData upgrade)
    {
        return upgrade != null && levels.TryGetValue(upgrade, out int level) ? level : 0;
    }

    public bool CanUpgrade(UpgradeData upgrade)
    {
        if (upgrade == null)
            return false;

        int currentLevel = GetLevel(upgrade);
        return currentLevel < upgrade.maxLevel;
    }

    public bool TryUpgrade(UpgradeData upgrade)
    {
        if (!CanUpgrade(upgrade))
            return false;

        levels[upgrade] = GetLevel(upgrade) + 1;
        return true;
    }

    public void ApplyTo(ref UnitStats stats)
    {
        foreach (KeyValuePair<UpgradeData, int> entry in levels)
        {
            UpgradeData upgrade = entry.Key;
            int level = entry.Value;

            if (upgrade == null || level <= 0)
                continue;

            upgrade.Apply(ref stats, level);
        }
    }
}
