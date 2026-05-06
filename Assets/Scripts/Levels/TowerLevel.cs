using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class TowerLevel : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private TMP_Text statusText;

    [Header("Audio")]
    [SerializeField] private AudioSource audioSource;

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

    private AudioClip earnCurrencyBeep;
    private AudioClip purchaseBeep;
    private AudioClip winBeep;

    private void Start()
    {
        AutoFindReferences();
        CreateAudioClips();

        if (sling != null)
            sling.EnemyDefeated += AddCurrency;

        if (archer != null)
            archer.EnemyDefeated += AddCurrency;

        if (mage != null)
            mage.EnemyDefeated += AddCurrency;

        UpdateUI("Click units/windows to buy.");
    }

    private void Update()
    {
        HandleRestartInput();

        if (!gameWon)
        {
            HandleKeyboardPurchases();
        }

        UpdateUI();
    }

    private void AutoFindReferences()
    {
        if (statusText == null)
            statusText = FindFirstObjectByType<TMP_Text>();

        if (audioSource == null)
            audioSource = GetComponent<AudioSource>();

        if (sling == null)
            sling = GameObject.Find("Sling")?.GetComponent<Unit>();

        if (archer == null)
            archer = GameObject.Find("Archer")?.GetComponent<Unit>();

        if (mage == null)
            mage = GameObject.Find("Mage")?.GetComponent<Unit>();
    }

    private void CreateAudioClips()
    {
        earnCurrencyBeep = CreateBeepClip(660f, 0.12f, 0.6f);
        purchaseBeep = CreateBeepClip(880f, 0.15f, 0.7f);
        winBeep = CreateBeepClip(1040f, 0.35f, 0.8f);
    }

    private void HandleRestartInput()
    {
        if (Keyboard.current == null)
            return;

        if (Keyboard.current.rKey.wasPressedThisFrame)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
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

        if (!gameWon)
        {
            PlaySound(earnCurrencyBeep);
        }

        CheckWinCondition();
    }

    private bool TrySpend(int cost)
    {
        if (currency < cost)
            return false;

        currency -= cost;
        PlaySound(purchaseBeep);
        return true;
    }

    public bool TryPurchase(PurchaseType purchaseType)
    {
        if (gameWon)
            return false;

        bool wasComplete = IsPurchaseComplete(purchaseType);

        switch (purchaseType)
        {
            case PurchaseType.BuyArcher:
                BuyArcher();
                break;

            case PurchaseType.BuyMage:
                BuyMage();
                break;

            case PurchaseType.SlingRockSize:
                BuySlingDamageUpgrade();
                break;

            case PurchaseType.SlingQuality:
                BuySlingFireRateUpgrade();
                break;

            case PurchaseType.ArcherArrowQuality:
                BuyArcherDamageUpgrade();
                break;

            case PurchaseType.ArcherQuiver:
                BuyArcherFireRateUpgrade();
                break;

            case PurchaseType.MageSpellLevel:
                BuyMageDamageUpgrade();
                break;

            case PurchaseType.MageManaRegen:
                BuyMageFireRateUpgrade();
                break;
        }

        UpdateUI();

        bool isComplete = IsPurchaseComplete(purchaseType);
        return !wasComplete && isComplete;
    }

    public bool IsPurchaseComplete(PurchaseType purchaseType)
    {
        switch (purchaseType)
        {
            case PurchaseType.BuyArcher:
                return archerBought;

            case PurchaseType.BuyMage:
                return mageBought;

            case PurchaseType.SlingRockSize:
                return slingDamageBought;

            case PurchaseType.SlingQuality:
                return slingFireRateBought;

            case PurchaseType.ArcherArrowQuality:
                return archerDamageBought;

            case PurchaseType.ArcherQuiver:
                return archerFireRateBought;

            case PurchaseType.MageSpellLevel:
                return mageDamageBought;

            case PurchaseType.MageManaRegen:
                return mageFireRateBought;

            default:
                return false;
        }
    }

    public void BuyArcher()
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

    public void BuyMage()
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

    public void BuySlingDamageUpgrade()
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

    public void BuySlingFireRateUpgrade()
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

    public void BuyArcherDamageUpgrade()
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

    public void BuyArcherFireRateUpgrade()
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

    public void BuyMageDamageUpgrade()
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

    public void BuyMageFireRateUpgrade()
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

        if (allComplete && !gameWon)
        {
            gameWon = true;
            PlaySound(winBeep);
        }
    }

    private int GetUpgradeCount()
    {
        int count = 0;

        if (slingDamageBought) count++;
        if (slingFireRateBought) count++;
        if (archerDamageBought) count++;
        if (archerFireRateBought) count++;
        if (mageDamageBought) count++;
        if (mageFireRateBought) count++;

        return count;
    }

    private void UpdateUI(string message = "")
    {
        if (statusText == null)
            return;

        if (gameWon)
        {
            statusText.text =
                $"YOU WIN!\n" +
                $"Ground floor fully upgraded.\n" +
                $"Currency: {currency}\n" +
                $"Press R to restart.";
            return;
        }

        statusText.text =
            $"Currency: {currency}\n" +
            $"Goal: buy all upgrades\n" +
            $"Grey unit = buy\n" +
            $"Window = upgrade\n" +
            $"Upgrades: {GetUpgradeCount()}/6\n" +
            $"{message}";
    }

    private AudioClip CreateBeepClip(float frequency, float duration, float volume)
    {
        int sampleRate = 44100;
        int sampleCount = Mathf.CeilToInt(sampleRate * duration);
        float[] samples = new float[sampleCount];

        for (int i = 0; i < sampleCount; i++)
        {
            float time = (float)i / sampleRate;
            samples[i] = Mathf.Sin(2f * Mathf.PI * frequency * time) * volume;
        }

        AudioClip clip = AudioClip.Create("GeneratedBeep", sampleCount, 1, sampleRate, false);
        clip.SetData(samples, 0);

        return clip;
    }

    private void PlaySound(AudioClip clip)
    {
        if (audioSource != null && clip != null)
        {
            audioSource.PlayOneShot(clip);
        }
    }
}