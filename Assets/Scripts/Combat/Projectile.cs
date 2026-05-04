using UnityEngine;

/// <summary>
/// Homing projectile aimed at a specific enemy.
/// Good enough for an idle game. The rocket scientists can calm down.
/// </summary>
public class Projectile : MonoBehaviour
{
    [SerializeField] private float defaultSpeed = 8f;
    [SerializeField] private float hitDistance = 0.1f;
    [SerializeField] private float lifetime = 5f;

    private Enemy target;
    private float damage;
    private float speed;

    public void Initialize(Enemy newTarget, float newDamage, float newSpeed, float sizeMultiplier)
    {
        target = newTarget;
        damage = newDamage;
        speed = newSpeed > 0f ? newSpeed : defaultSpeed;
        transform.localScale *= Mathf.Max(0.01f, sizeMultiplier);

        Destroy(gameObject, lifetime);
    }

    private void Update()
    {
        if (target == null)
        {
            Destroy(gameObject);
            return;
        }

        transform.position = Vector2.MoveTowards(
            transform.position,
            target.transform.position,
            speed * Time.deltaTime
        );

        float distance = Vector2.Distance(transform.position, target.transform.position);

        if (distance <= hitDistance)
        {
            target.TakeDamage(damage);
            Destroy(gameObject);
        }
    }
}
