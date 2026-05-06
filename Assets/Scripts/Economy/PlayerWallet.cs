using UnityEngine;

/// <summary>
/// Tiny placeholder wallet. Replace with a proper saveable economy later.
/// </summary>
public class PlayerWallet : MonoBehaviour
{
    public static PlayerWallet Instance { get; private set; }

    [SerializeField] private int gold;

    public int Gold => gold;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    public void AddGold(int amount)
    {
        gold += Mathf.Max(0, amount);
    }

    public bool TrySpendGold(int amount)
    {
        if (amount < 0 || gold < amount)
            return false;

        gold -= amount;
        return true;
    }
}
