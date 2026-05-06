using UnityEngine;

/// <summary>
/// Draft purchase/apply point for upgrades.
/// Real currency checks should hook into PlayerWallet or EconomyManager.
/// </summary>
public class UpgradeManager : MonoBehaviour
{
    public bool TryBuyUpgrade(UnitUpgradeState upgradeState, UpgradeData upgrade)
    {
        if (upgradeState == null || upgrade == null)
            return false;

        int currentLevel = upgradeState.GetLevel(upgrade);
        int cost = upgrade.GetCostForNextLevel(currentLevel);

        // TODO: Check player currency before buying.
        // Example:
        // if (!PlayerWallet.Instance.TrySpendGold(cost)) return false;

        bool upgraded = upgradeState.TryUpgrade(upgrade);

        if (!upgraded)
            return false;

        Debug.Log($"Bought {upgrade.displayName} for {cost} gold.");
        return true;
    }
}
