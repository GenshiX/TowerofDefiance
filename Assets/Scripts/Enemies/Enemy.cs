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

    [Header("Health Bar")]
    [SerializeField] private Transform healthBarFill;
    [SerializeField] private Color fullHealthColor = Color.blue;
    [SerializeField] private Color lowHealthColor = Color.red;

    private float currentHealth;
    private SpriteRenderer spriteRenderer;
    private Color originalColor;

    private Vector3 originalHealthBarScale;
    private Vector3 originalHealthBarPosition;
    private SpriteRenderer healthBarFillRenderer;

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

        if (healthBarFill != null)
        {
            originalHealthBarScale = healthBarFill.localScale;
            originalHealthBarPosition = healthBarFill.localPosition;
            healthBarFillRenderer = healthBarFill.GetComponent<SpriteRenderer>();
        }

        UpdateHealthBar();
    }

    public int TakeDamage(float damage)
    {
        currentHealth -= damage;

        if (currentHealth < 0f)
        {
            currentHealth = 0f;
        }

        Flash();
        UpdateHealthBar();

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
        UpdateHealthBar();

        return reward;
    }

    private void UpdateHealthBar()
    {
        if (healthBarFill == null)
            return;

        float healthPercent = Mathf.Clamp01(currentHealth / maxHealth);

        float newScaleX = originalHealthBarScale.x * healthPercent;
        float xOffset = (originalHealthBarScale.x - newScaleX) * 0.5f;

        healthBarFill.localScale = new Vector3(
            newScaleX,
            originalHealthBarScale.y,
            originalHealthBarScale.z
        );

        // Keeps the LEFT side fixed instead of shrinking from both sides.
        healthBarFill.localPosition = new Vector3(
            originalHealthBarPosition.x - xOffset,
            originalHealthBarPosition.y,
            originalHealthBarPosition.z
        );

        if (healthBarFillRenderer != null)
        {
            healthBarFillRenderer.color = Color.Lerp(lowHealthColor, fullHealthColor, healthPercent);
        }
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