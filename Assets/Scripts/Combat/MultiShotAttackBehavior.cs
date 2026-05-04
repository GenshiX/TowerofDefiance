using UnityEngine;

/// <summary>
/// Fires multiple projectiles. Useful for missile quantity, machine gun bursts, etc.
/// </summary>
public class MultiShotAttackBehavior : AttackBehavior
{
    [SerializeField] private Projectile projectilePrefab;
    [SerializeField] private Transform firePoint;

    public override void Attack(Enemy target, UnitStats stats)
    {
        if (target == null || projectilePrefab == null)
            return;

        int count = Mathf.Max(1, stats.ProjectileCount);
        Vector3 spawnPosition = firePoint != null ? firePoint.position : transform.position;

        for (int i = 0; i < count; i++)
        {
            Projectile projectile = Instantiate(projectilePrefab, spawnPosition, Quaternion.identity);
            projectile.Initialize(target, stats.Damage, stats.ProjectileSpeed, stats.ProjectileSize);
        }
    }
}
