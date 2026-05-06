using UnityEngine;

public class ProjectileAttackBehavior : AttackBehavior
{
    public override void Attack(Enemy target, UnitStats stats)
    {
        // Not used in the current Ground-floor idle slice.
        // Placeholder implementation so the old skeleton behavior compiles.
        Debug.Log("ProjectileAttackBehavior is not implemented yet.");
    }
}