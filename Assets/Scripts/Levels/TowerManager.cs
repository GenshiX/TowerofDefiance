using UnityEngine;

/// <summary>
/// Central place for all tower levels.
/// Useful later for unlocks, save/load, and UI lookup.
/// </summary>
public class TowerManager : MonoBehaviour
{
    [SerializeField] private TowerLevel[] levels;

    public TowerLevel GetLevel(int index)
    {
        if (levels == null || index < 0 || index >= levels.Length)
            return null;

        return levels[index];
    }
}
