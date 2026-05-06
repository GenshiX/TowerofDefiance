using System;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [Header("Projectile Settings")]
    [SerializeField] private float lifetime = 5f;

    private Enemy target;
    private float speed;
    private float damage;
    private Action<int> onEnemyDefeated;
    private bool hasHit;

    public void Initialize(
        Enemy target,
        float speed,
        float damage,
        Action<int> onEnemyDefeated
    )
    {
        this.target = target;
        this.speed = speed;
        this.damage = damage;
        this.onEnemyDefeated = onEnemyDefeated;
        hasHit = false;
    }

    private void Update()
    {
        lifetime -= Time.deltaTime;

        if (lifetime <= 0f)
        {
            Destroy(gameObject);
            return;
        }

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
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (hasHit)
            return;

        if (target == null)
            return;

        if (other.gameObject != target.gameObject)
            return;

        hasHit = true;

        int reward = target.TakeDamage(damage);

        if (reward > 0)
        {
            onEnemyDefeated?.Invoke(reward);
        }

        Destroy(gameObject);
    }
}