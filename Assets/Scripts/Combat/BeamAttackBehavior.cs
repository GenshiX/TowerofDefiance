using UnityEngine;

/// <summary>
/// Draft behavior for instant beam-style attacks, such as Orbital Laser.
/// Visual beam effects can be added later without changing Unit.cs.
/// </summary>
public class BeamAttackBehavior : AttackBehavior
{
    public override void Attack(Enemy target, UnitStats stats)
    {
        if (target == null)
            return;

        float focusedDamage = stats.Damage * stats.BeamFocusMultiplier;
        target.TakeDamage(focusedDamage);
    }
}
