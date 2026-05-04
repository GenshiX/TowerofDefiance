using UnityEngine;

/// <summary>
/// Simple instant damage. Good for testing before projectiles exist.
/// </summary>
public class DirectAttackBehavior : AttackBehavior
{
    public override void Attack(Enemy target, UnitStats stats)
    {
        if (target == null)
            return;

        target.TakeDamage(stats.Damage);
    }
}
