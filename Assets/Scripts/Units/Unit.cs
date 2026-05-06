using System;
using UnityEngine;

public class Unit : MonoBehaviour
{
    [Header("Unit State")]
    [SerializeField] private string unitName = "Unit";
    [SerializeField] private bool startsUnlocked = false;

    [Header("Combat")]
    [SerializeField] private Enemy targetEnemy;
    [SerializeField] private Projectile projectilePrefab;
    [SerializeField] private Transform firePoint;

    [SerializeField] private float damage = 1f;
    [SerializeField] private float fireRate = 1f;
    [SerializeField] private float projectileSpeed = 6f;

    private bool unlocked;
    private float fireCooldown;

    public string UnitName => unitName;
    public bool IsUnlocked => unlocked;
    public float Damage => damage;
    public float FireRate => fireRate;

    public event Action<int> EnemyDefeated;

    private void Awake()
    {
        unlocked = startsUnlocked;

        if (firePoint == null)
        {
            firePoint = transform;
        }

        gameObject.SetActive(unlocked);
    }

    private void Update()
    {
        if (!unlocked)
            return;

        if (targetEnemy == null || projectilePrefab == null)
            return;

        fireCooldown -= Time.deltaTime;

        if (fireCooldown <= 0f)
        {
            Fire();
            fireCooldown = 1f / fireRate;
        }
    }

    public void Unlock()
    {
        unlocked = true;
        gameObject.SetActive(true);
    }

    public void IncreaseDamage(float amount)
    {
        damage += amount;
    }

    public void IncreaseFireRate(float amount)
    {
        fireRate += amount;
    }

    private void Fire()
    {
        Projectile projectile = Instantiate(
            projectilePrefab,
            firePoint.position,
            Quaternion.identity
        );

        projectile.gameObject.SetActive(true);

        projectile.Initialize(
            targetEnemy,
            projectileSpeed,
            damage,
            HandleEnemyDefeated
        );
    }

    private void HandleEnemyDefeated(int currencyReward)
    {
        EnemyDefeated?.Invoke(currencyReward);
    }
}