using System;
using UnityEngine;

/// <summary>
/// Runtime enemy. Handles health, damage, and death event.
/// </summary>
public class Enemy : Damageable
{
    public event Action<Enemy> Died;

    public EnemyData Data { get; private set; }
    public float CurrentHealth { get; private set; }
    public float MaxHealth { get; private set; }
    public int GoldReward { get; private set; }

    public void Initialize(EnemyData data, int spawnIndex)
    {
        Data = data;

        if (Data == null)
        {
            MaxHealth = 10f;
            GoldReward = 1;
        }
        else
        {
            float healthMultiplier = 1f + Data.healthGrowthPerSpawn * Mathf.Max(0, spawnIndex - 1);
            float rewardMultiplier = 1f + Data.rewardGrowthPerSpawn * Mathf.Max(0, spawnIndex - 1);

            MaxHealth = Data.baseHealth * healthMultiplier;
            GoldReward = Mathf.RoundToInt(Data.goldReward * rewardMultiplier);
        }

        CurrentHealth = MaxHealth;
    }

    public override void TakeDamage(float amount)
    {
        if (amount <= 0f)
            return;

        CurrentHealth -= amount;

        if (CurrentHealth <= 0f)
            Die();
    }

    private void Die()
    {
        Died?.Invoke(this);
        Destroy(gameObject);
    }
}
