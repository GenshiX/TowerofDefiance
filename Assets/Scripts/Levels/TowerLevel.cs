using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class TowerLevel : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private TMP_Text statusText;

    [Header("Currency")]
    [SerializeField] private int currency = 0;

    [Header("Units")]
    [SerializeField] private Unit sling;
    [SerializeField] private Unit archer;
    [SerializeField] private Unit mage;

    [Header("Unit Costs")]
    [SerializeField] private int archerCost = 150;
    [SerializeField] private int mageCost = 400;

    [Header("Upgrade Costs")]
    [SerializeField] private int slingDamageUpgradeCost = 100;
    [SerializeField] private int slingFireRateUpgradeCost = 150;

    [SerializeField] private int archerDamageUpgradeCost = 200;
    [SerializeField] private int archerFireRateUpgradeCost = 250;

    [SerializeField] private int mageDamageUpgradeCost = 300;
    [SerializeField] private int mageFireRateUpgradeCost = 350;

    [Header("Upgrade Values")]
    [SerializeField] private float damageUpgradeAmount = 1f;
    [SerializeField] private float fireRateUpgradeAmount = 0.5f;

    private bool archerBought;
    private bool mageBought;

    private bool slingDamageBought;
    private bool slingFireRateBought;

    private bool archerDamageBought;
    private bool archerFireRateBought;

    private bool mageDamageBought;
    private bool mageFireRateBought;

    private bool gameWon;

    private void Start()
    {
        AutoFindReferences();

        if (sling != null)
            sling.EnemyDefeated += AddCurrency;

        if (archer != null)
            archer.EnemyDefeated += AddCurrency;

        if (mage != null)
            mage.EnemyDefeated += AddCurrency;

        UpdateUI("Buy all Ground upgrades to win.");
    }

    private void Update()
    {
        if (gameWon)
            return;

        HandleKeyboardPurchases();
        UpdateUI();
    }

    private void AutoFindReferences()
    {
        if (statusText == null)
            statusText = FindFirstObjectByType<TMP_Text>();

        if (sling == null)
            sling = GameObject.Find("Sling")?.GetComponent<Unit>();

        if (archer == null)
            archer = GameObject.Find("Archer")?.GetComponent<Unit>();

        if (mage == null)
            mage = GameObject.Find("Mage")?.GetComponent<Unit>();
    }

    private void HandleKeyboardPurchases()
    {
        if (Keyboard.current == null)
            return;

        if (Keyboard.current.aKey.wasPressedThisFrame)
            BuyArcher();

        if (Keyboard.current.mKey.wasPressedThisFrame)
            BuyMage();

        if (Keyboard.current.digit1Key.wasPressedThisFrame)
            BuySlingDamageUpgrade();

        if (Keyboard.current.digit2Key.wasPressedThisFrame)
            BuySlingFireRateUpgrade();

        if (Keyboard.current.digit3Key.wasPressedThisFrame)
            BuyArcherDamageUpgrade();

        if (Keyboard.current.digit4Key.wasPressedThisFrame)
            BuyArcherFireRateUpgrade();

        if (Keyboard.current.digit5Key.wasPressedThisFrame)
            BuyMageDamageUpgrade();

        if (Keyboard.current.digit6Key.wasPressedThisFrame)
            BuyMageFireRateUpgrade();
    }

    private void AddCurrency(int amount)
    {
        currency += amount;
        CheckWinCondition();
    }

    private bool TrySpend(int cost)
    {
        if (currency < cost)
            return false;

        currency -= cost;
        return true;
    }

    private void BuyArcher()
    {
        if (archerBought)
            return;

        if (!TrySpend(archerCost))
            return;

        archerBought = true;

        if (archer != null)
            archer.Unlock();

        UpdateUI("Archer bought!");
        CheckWinCondition();
    }

    private void BuyMage()
    {
        if (mageBought)
            return;

        if (!TrySpend(mageCost))
            return;

        mageBought = true;

        if (mage != null)
            mage.Unlock();

        UpdateUI("Mage bought!");
        CheckWinCondition();
    }

    private void BuySlingDamageUpgrade()
    {
        if (slingDamageBought)
            return;

        if (!TrySpend(slingDamageUpgradeCost))
            return;

        slingDamageBought = true;

        if (sling != null)
            sling.IncreaseDamage(damageUpgradeAmount);

        CheckWinCondition();
    }

    private void BuySlingFireRateUpgrade()
    {
        if (slingFireRateBought)
            return;

        if (!TrySpend(slingFireRateUpgradeCost))
            return;

        slingFireRateBought = true;

        if (sling != null)
            sling.IncreaseFireRate(fireRateUpgradeAmount);

        CheckWinCondition();
    }

    private void BuyArcherDamageUpgrade()
    {
        if (archerDamageBought)
            return;

        if (!archerBought)
            return;

        if (!TrySpend(archerDamageUpgradeCost))
            return;

        archerDamageBought = true;

        if (archer != null)
            archer.IncreaseDamage(damageUpgradeAmount);

        CheckWinCondition();
    }

    private void BuyArcherFireRateUpgrade()
    {
        if (archerFireRateBought)
            return;

        if (!archerBought)
            return;

        if (!TrySpend(archerFireRateUpgradeCost))
            return;

        archerFireRateBought = true;

        if (archer != null)
            archer.IncreaseFireRate(fireRateUpgradeAmount);

        CheckWinCondition();
    }

    private void BuyMageDamageUpgrade()
    {
        if (mageDamageBought)
            return;

        if (!mageBought)
            return;

        if (!TrySpend(mageDamageUpgradeCost))
            return;

        mageDamageBought = true;

        if (mage != null)
            mage.IncreaseDamage(damageUpgradeAmount);

        CheckWinCondition();
    }

    private void BuyMageFireRateUpgrade()
    {
        if (mageFireRateBought)
            return;

        if (!mageBought)
            return;

        if (!TrySpend(mageFireRateUpgradeCost))
            return;

        mageFireRateBought = true;

        if (mage != null)
            mage.IncreaseFireRate(fireRateUpgradeAmount);

        CheckWinCondition();
    }

    private void CheckWinCondition()
    {
        bool allComplete =
            archerBought &&
            mageBought &&
            slingDamageBought &&
            slingFireRateBought &&
            archerDamageBought &&
            archerFireRateBought &&
            mageDamageBought &&
            mageFireRateBought;

        if (allComplete)
        {
            gameWon = true;
            statusText.text =
                "YOU WIN!\n" +
                "Ground floor fully upgraded.\n\n" +
                "Sling, Archer, and Mage are fully built.";
        }
    }

    private string BoughtText(bool bought, int cost)
    {
        return bought ? "Bought" : $"Cost {cost}";
    }

    private void UpdateUI(string message = "")
    {
        if (statusText == null)
            return;

        statusText.text =
            $"Currency: {currency}\n" +
            $"Goal: Fully upgrade the Ground floor\n\n" +

            $"Units:\n" +
            $"A - Archer: {BoughtText(archerBought, archerCost)}\n" +
            $"M - Mage: {BoughtText(mageBought, mageCost)}\n\n" +

            $"Upgrades:\n" +
            $"1 - Sling Rock Size: {BoughtText(slingDamageBought, slingDamageUpgradeCost)}\n" +
            $"2 - Sling Quality: {BoughtText(slingFireRateBought, slingFireRateUpgradeCost)}\n" +
            $"3 - Archer Arrow Quality: {BoughtText(archerDamageBought, archerDamageUpgradeCost)}\n" +
            $"4 - Archer Quiver: {BoughtText(archerFireRateBought, archerFireRateUpgradeCost)}\n" +
            $"5 - Mage Spell Level: {BoughtText(mageDamageBought, mageDamageUpgradeCost)}\n" +
            $"6 - Mage Mana Regen: {BoughtText(mageFireRateBought, mageFireRateUpgradeCost)}\n\n" +

            $"Stats:\n" +
            $"Sling Damage: {(sling != null ? sling.Damage.ToString("0.0") : "?")} | Rate: {(sling != null ? sling.FireRate.ToString("0.0") : "?")}\n" +
            $"Archer Damage: {(archer != null ? archer.Damage.ToString("0.0") : "?")} | Rate: {(archer != null ? archer.FireRate.ToString("0.0") : "?")}\n" +
            $"Mage Damage: {(mage != null ? mage.Damage.ToString("0.0") : "?")} | Rate: {(mage != null ? mage.FireRate.ToString("0.0") : "?")}\n\n" +
            message;
    }
}