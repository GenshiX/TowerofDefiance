using UnityEngine;

/// <summary>
/// Defines how a unit applies damage.
/// Examples: instant hit, projectile, beam, multishot.
/// </summary>
public abstract class AttackBehavior : MonoBehaviour
{
    public abstract void Attack(Enemy target, UnitStats stats);
}
