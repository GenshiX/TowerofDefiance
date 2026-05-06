using System.Collections;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [Header("Enemy Settings")]
    [SerializeField] private string enemyName = "Enemy";
    [SerializeField] private float maxHealth = 10f;
    [SerializeField] private int currencyReward = 10;

    [Header("Visual Feedback")]
    [SerializeField] private Color hitColor = Color.red;
    [SerializeField] private float flashDuration = 0.08f;

    private float currentHealth;
    private SpriteRenderer spriteRenderer;
    private Color originalColor;

    public string EnemyName => enemyName;
    public float CurrentHealth => currentHealth;
    public float MaxHealth => maxHealth;
    public int CurrencyReward => currencyReward;

    private void Awake()
    {
        currentHealth = maxHealth;

        spriteRenderer = GetComponent<SpriteRenderer>();

        if (spriteRenderer != null)
        {
            originalColor = spriteRenderer.color;
        }
    }

    public int TakeDamage(float damage)
    {
        currentHealth -= damage;
        Flash();

        if (currentHealth <= 0f)
        {
            return Defeat();
        }

        return 0;
    }

    private int Defeat()
    {
        int reward = currencyReward;
        currentHealth = maxHealth;
        return reward;
    }

    private void Flash()
    {
        if (spriteRenderer == null)
            return;

        StopAllCoroutines();
        StartCoroutine(FlashRoutine());
    }

    private IEnumerator FlashRoutine()
    {
        spriteRenderer.color = hitColor;
        yield return new WaitForSeconds(flashDuration);
        spriteRenderer.color = originalColor;
    }
}