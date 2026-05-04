using UnityEngine;

/// <summary>
/// Base class for anything that can take damage.
/// Keep this small. Health, death, and rewards can live in derived classes.
/// </summary>
public abstract class Damageable : MonoBehaviour
{
    public abstract void TakeDamage(float amount);
}
