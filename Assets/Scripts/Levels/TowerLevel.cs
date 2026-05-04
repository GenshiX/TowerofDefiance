using UnityEngine;

/// <summary>
/// Owns the current enemy for a single tower level/lane.
/// Units assigned to this level attack CurrentEnemy.
/// </summary>
public class TowerLevel : MonoBehaviour
{
    [SerializeField] private TowerLevelData levelData;
    [SerializeField] private Enemy enemyPrefab;
    [SerializeField] private Transform enemySpawnPoint;

    private int enemySpawnCount;
    private int enemyRotationIndex;

    public Enemy CurrentEnemy { get; private set; }
    public TowerLevelData LevelData => levelData;

    private void Start()
    {
        SpawnNextEnemy();
    }

    public void SpawnNextEnemy()
    {
        if (enemyPrefab == null)
        {
            Debug.LogWarning($"{name} has no enemy prefab assigned.");
            return;
        }

        EnemyData nextEnemyData = GetNextEnemyData();
        Vector3 spawnPosition = enemySpawnPoint != null ? enemySpawnPoint.position : transform.position;

        Enemy enemy = Instantiate(enemyPrefab, spawnPosition, Quaternion.identity);
        enemySpawnCount++;
        enemy.Initialize(nextEnemyData, enemySpawnCount);
        enemy.Died += HandleEnemyDied;

        CurrentEnemy = enemy;
    }

    private EnemyData GetNextEnemyData()
    {
        if (levelData == null || levelData.enemyRotation == null || levelData.enemyRotation.Length == 0)
            return null;

        EnemyData data = levelData.enemyRotation[enemyRotationIndex];
        enemyRotationIndex = (enemyRotationIndex + 1) % levelData.enemyRotation.Length;
        return data;
    }

    private void HandleEnemyDied(Enemy enemy)
    {
        if (enemy != null)
            enemy.Died -= HandleEnemyDied;

        if (CurrentEnemy == enemy)
            CurrentEnemy = null;

        // TODO: Send reward through EconomyManager or PlayerWallet.
        // Example: PlayerWallet.Instance.AddGold(enemy.GoldReward);

        SpawnNextEnemy();
    }
}
