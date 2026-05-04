using UnityEngine;

/// <summary>
/// Runtime unit behavior.
/// It does not search for enemies. It asks its assigned TowerLevel for the current enemy.
/// </summary>
public class Unit : MonoBehaviour
{
    [SerializeField] private TowerLevel assignedLevel;
    [SerializeField] private UnitData unitData;
    [SerializeField] private UnitUpgradeState upgradeState;
    [SerializeField] private AttackBehavior attackBehavior;

    private float attackCooldown;

    private void Awake()
    {
        if (upgradeState == null)
            upgradeState = GetComponent<UnitUpgradeState>();

        if (attackBehavior == null)
            attackBehavior = GetComponent<AttackBehavior>();

        if (upgradeState != null)
            upgradeState.Initialize(unitData);
    }

    private void Update()
    {
        attackCooldown -= Time.deltaTime;

        if (assignedLevel == null || attackBehavior == null)
            return;

        Enemy target = assignedLevel.CurrentEnemy;

        if (target == null)
            return;

        UnitStats stats = UnitStats.From(unitData, upgradeState);

        if (attackCooldown <= 0f)
        {
            attackBehavior.Attack(target, stats);
            attackCooldown = 1f / stats.AttackRate;
        }
    }

    public void AssignLevel(TowerLevel level)
    {
        assignedLevel = level;
    }

    public UnitData GetUnitData()
    {
        return unitData;
    }
}
