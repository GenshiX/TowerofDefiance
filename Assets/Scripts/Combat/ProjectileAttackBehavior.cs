using UnityEngine;

/// <summary>
/// Fires one projectile toward the current enemy.
/// </summary>
public class ProjectileAttackBehavior : AttackBehavior
{
    [SerializeField] private Projectile projectilePrefab;
    [SerializeField] private Transform firePoint;

    public override void Attack(Enemy target, UnitStats stats)
    {
        if (target == null || projectilePrefab == null)
            return;

        Vector3 spawnPosition = firePoint != null ? firePoint.position : transform.position;

        Projectile projectile = Instantiate(projectilePrefab, spawnPosition, Quaternion.identity);
        projectile.Initialize(target, stats.Damage, stats.ProjectileSpeed, stats.ProjectileSize);
    }
}
