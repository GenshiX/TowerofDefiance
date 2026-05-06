using System;
using TMPro;
using UnityEngine;

public class GroundCombatGame : MonoBehaviour
{
    [Header("Scene References")]
    [SerializeField] private TMP_Text statusText;
    [SerializeField] private AudioSource audioSource;

    [Header("Enemy Visuals")]
    [SerializeField] private Transform goblin;
    [SerializeField] private Transform goblinGlider;
    [SerializeField] private Transform griffin;

    [Header("Game Settings")]
    [SerializeField] private float gameTime = 90f;
    [SerializeField] private int bonesToWin = 1000;

    [Header("Sling")]
    [SerializeField] private Transform sling;
    [SerializeField] private Transform slingProjectile;
    [SerializeField] private float slingDamage = 1f;
    [SerializeField] private float slingFireRate = 1f;
    [SerializeField] private float slingProjectileSpeed = 6f;

    [Header("Archer")]
    [SerializeField] private Transform archer;
    [SerializeField] private Transform arrowProjectile;
    [SerializeField] private int archerUnlockCost = 150;
    [SerializeField] private float archerDamage = 1f;
    [SerializeField] private float archerFireRate = 1.5f;
    [SerializeField] private float archerProjectileSpeed = 7f;

    [Header("Mage")]
    [SerializeField] private Transform mage;
    [SerializeField] private Transform mageProjectile;
    [SerializeField] private int mageUnlockCost = 400;
    [SerializeField] private float mageDamage = 3f;
    [SerializeField] private float mageFireRate = 0.6f;
    [SerializeField] private float mageProjectileSpeed = 5f;

    private Transform activeEnemy;
    private string activeEnemyName;
    private float enemyHealth;
    private float enemyMaxHealth;
    private int enemyReward;
    private int enemyIndex;

    private float timer;
    private int kills;
    private int bones;
    private bool gameOver;

    private bool archerUnlocked;
    private bool mageUnlocked;

    private float slingCooldown;
    private float archerCooldown;
    private float mageCooldown;

    private AudioClip hitBeep;

    private void Start()
    {
        AutoFindReferences();

        timer = gameTime;
        hitBeep = CreateBeepClip();

        HideProjectileTemplates();
        HideAllEnemies();

        if (sling != null)
            sling.gameObject.SetActive(true);

        if (archer != null)
            archer.gameObject.SetActive(false);

        if (mage != null)
            mage.gameObject.SetActive(false);

        SpawnEnemyByIndex(0);

        UpdateUI("Earn enough Bones before time runs out!");
    }

    private void Update()
    {
        if (gameOver)
            return;

        timer -= Time.deltaTime;

        if (timer <= 0f)
        {
            LoseGame();
            return;
        }

        UnlockUnitsIfAffordable();

        UpdateSling();
        UpdateArcher();
        UpdateMage();

        UpdateUI();
    }

    private void AutoFindReferences()
    {
        if (goblin == null)
            goblin = GameObject.Find("Goblin")?.transform;

        if (goblinGlider == null)
            goblinGlider = GameObject.Find("Goblin Glider")?.transform;

        if (griffin == null)
            griffin = GameObject.Find("Griffin")?.transform;

        if (sling == null)
            sling = GameObject.Find("Sling")?.transform;

        if (slingProjectile == null)
            slingProjectile = GameObject.Find("Sling Projectile")?.transform;

        if (archer == null)
            archer = GameObject.Find("Archer")?.transform;

        if (arrowProjectile == null)
            arrowProjectile = GameObject.Find("Arrow")?.transform;

        if (mage == null)
            mage = GameObject.Find("Mage")?.transform;

        if (mageProjectile == null)
            mageProjectile = GameObject.Find("Lighting")?.transform;

        if (mageProjectile == null)
            mageProjectile = GameObject.Find("Lightning")?.transform;

        if (statusText == null)
            statusText = FindFirstObjectByType<TMP_Text>();

        if (audioSource == null)
            audioSource = GetComponent<AudioSource>();
    }

    private void HideProjectileTemplates()
    {
        if (slingProjectile != null)
            slingProjectile.gameObject.SetActive(false);

        if (arrowProjectile != null)
            arrowProjectile.gameObject.SetActive(false);

        if (mageProjectile != null)
            mageProjectile.gameObject.SetActive(false);
    }

    private void HideAllEnemies()
    {
        if (goblin != null)
            goblin.gameObject.SetActive(false);

        if (goblinGlider != null)
            goblinGlider.gameObject.SetActive(false);

        if (griffin != null)
            griffin.gameObject.SetActive(false);
    }

    private void SpawnEnemyByIndex(int index)
    {
        HideAllEnemies();

        enemyIndex = index % 3;

        if (enemyIndex == 0)
        {
            activeEnemy = goblin;
            activeEnemyName = "Goblin";
            enemyMaxHealth = 3f;
            enemyReward = 25;
        }
        else if (enemyIndex == 1)
        {
            activeEnemy = goblinGlider;
            activeEnemyName = "Goblin Glider";
            enemyMaxHealth = 6f;
            enemyReward = 50;
        }
        else
        {
            activeEnemy = griffin;
            activeEnemyName = "Griffin";
            enemyMaxHealth = 12f;
            enemyReward = 100;
        }

        enemyHealth = enemyMaxHealth;

        if (activeEnemy != null)
        {
            activeEnemy.gameObject.SetActive(true);
            EnsureEnemyCollider(activeEnemy.gameObject);
            ResetEnemyColor();
        }
    }

    private void UnlockUnitsIfAffordable()
    {
        if (!archerUnlocked && bones >= archerUnlockCost)
        {
            archerUnlocked = true;

            if (archer != null)
                archer.gameObject.SetActive(true);

            UpdateUI("Archer unlocked!");
        }

        if (!mageUnlocked && bones >= mageUnlockCost)
        {
            mageUnlocked = true;

            if (mage != null)
                mage.gameObject.SetActive(true);

            UpdateUI("Mage unlocked!");
        }
    }

    private void UpdateSling()
    {
        slingCooldown -= Time.deltaTime;

        if (slingCooldown <= 0f)
        {
            FireProjectile(sling, slingProjectile, slingDamage, slingProjectileSpeed);
            slingCooldown = 1f / slingFireRate;
        }
    }

    private void UpdateArcher()
    {
        if (!archerUnlocked)
            return;

        archerCooldown -= Time.deltaTime;

        if (archerCooldown <= 0f)
        {
            FireProjectile(archer, arrowProjectile, archerDamage, archerProjectileSpeed);
            archerCooldown = 1f / archerFireRate;
        }
    }

    private void UpdateMage()
    {
        if (!mageUnlocked)
            return;

        mageCooldown -= Time.deltaTime;

        if (mageCooldown <= 0f)
        {
            FireProjectile(mage, mageProjectile, mageDamage, mageProjectileSpeed);
            mageCooldown = 1f / mageFireRate;
        }
    }

    private void FireProjectile(Transform unit, Transform projectileTemplate, float damage, float speed)
    {
        if (unit == null || activeEnemy == null || projectileTemplate == null)
            return;

        Transform projectile = Instantiate(
            projectileTemplate,
            unit.position,
            Quaternion.identity
        );

        projectile.gameObject.SetActive(true);
        EnsureProjectilePhysics(projectile.gameObject);

        ProjectileMover mover = projectile.gameObject.AddComponent<ProjectileMover>();
        mover.Initialize(activeEnemy, speed, () => OnProjectileHit(damage));
    }

    private void OnProjectileHit(float damage)
    {
        if (gameOver)
            return;

        PlayHitSound();

        enemyHealth -= damage;
        FlashEnemy();

        if (enemyHealth <= 0f)
        {
            KillEnemy();
        }
    }

    private void KillEnemy()
    {
        kills++;
        bones += enemyReward;

        if (bones >= bonesToWin)
        {
            WinGame();
            return;
        }

        SpawnEnemyByIndex(enemyIndex + 1);
        UpdateUI($"{activeEnemyName} appeared!");
    }

    private void FlashEnemy()
    {
        if (activeEnemy == null)
            return;

        SpriteRenderer renderer = activeEnemy.GetComponent<SpriteRenderer>();

        if (renderer != null)
        {
            renderer.color = Color.red;
            Invoke(nameof(ResetEnemyColor), 0.08f);
        }
    }

    private void ResetEnemyColor()
    {
        if (activeEnemy == null)
            return;

        SpriteRenderer renderer = activeEnemy.GetComponent<SpriteRenderer>();

        if (renderer != null)
        {
            renderer.color = Color.white;
        }
    }

    private void EnsureEnemyCollider(GameObject enemyObject)
    {
        CircleCollider2D collider = enemyObject.GetComponent<CircleCollider2D>();

        if (collider == null)
            collider = enemyObject.AddComponent<CircleCollider2D>();

        collider.isTrigger = true;
    }

    private void EnsureProjectilePhysics(GameObject projectile)
    {
        CircleCollider2D collider = projectile.GetComponent<CircleCollider2D>();

        if (collider == null)
            collider = projectile.AddComponent<CircleCollider2D>();

        collider.isTrigger = true;

        Rigidbody2D rb = projectile.GetComponent<Rigidbody2D>();

        if (rb == null)
            rb = projectile.AddComponent<Rigidbody2D>();

        rb.bodyType = RigidbodyType2D.Kinematic;
    }

    private void PlayHitSound()
    {
        if (audioSource != null && hitBeep != null)
            audioSource.PlayOneShot(hitBeep);
    }

    private AudioClip CreateBeepClip()
    {
        int sampleRate = 44100;
        float duration = 0.08f;
        float frequency = 880f;

        int sampleCount = Mathf.CeilToInt(sampleRate * duration);
        float[] samples = new float[sampleCount];

        for (int i = 0; i < sampleCount; i++)
        {
            float time = (float)i / sampleRate;
            samples[i] = Mathf.Sin(2f * Mathf.PI * frequency * time) * 0.2f;
        }

        AudioClip clip = AudioClip.Create("HitBeep", sampleCount, 1, sampleRate, false);
        clip.SetData(samples, 0);

        return clip;
    }

    private void WinGame()
    {
        gameOver = true;
        UpdateUI("YOU WIN! You earned enough Bones.");
    }

    private void LoseGame()
    {
        gameOver = true;
        UpdateUI("YOU LOSE! Time ran out before you earned enough Bones.");
    }

    private void UpdateUI(string message = "")
    {
        if (statusText == null)
            return;

        string archerStatus = archerUnlocked ? "Active" : $"Unlocks at {archerUnlockCost} Bones";
        string mageStatus = mageUnlocked ? "Active" : $"Unlocks at {mageUnlockCost} Bones";

        statusText.text =
            $"Bones: {bones}/{bonesToWin}\n" +
            $"Kills: {kills}\n" +
            $"Time: {Mathf.CeilToInt(timer)}\n" +
            $"Enemy: {activeEnemyName}\n" +
            $"Enemy HP: {Mathf.CeilToInt(enemyHealth)}/{enemyMaxHealth}\n" +
            $"Reward: {enemyReward} Bones\n" +
            $"Sling: Active\n" +
            $"Archer: {archerStatus}\n" +
            $"Mage: {mageStatus}\n" +
            message;
    }
}

public class ProjectileMover : MonoBehaviour
{
    private Transform target;
    private float speed;
    private Action onHit;
    private bool hasHit;

    public void Initialize(Transform target, float speed, Action onHit)
    {
        this.target = target;
        this.speed = speed;
        this.onHit = onHit;
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
            target.position,
            speed * Time.deltaTime
        );
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (hasHit)
            return;

        if (target == null)
            return;

        if (other.transform != target)
            return;

        hasHit = true;
        onHit?.Invoke();
        Destroy(gameObject);
    }
}