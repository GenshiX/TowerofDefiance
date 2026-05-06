using UnityEngine;

public abstract class UnitAI : MonoBehaviour
{
    [SerializeField] protected float detectionRange = 5f;
    [SerializeField] protected LayerMask enemyLayer;

    protected Transform currentTarget;

    protected virtual void Update()
    {
        if (currentTarget == null || !IsTargetValid(currentTarget))
        {
            FindTarget();
        }

        if (currentTarget != null)
        {
            Act();
        }
    }

    protected virtual void FindTarget()
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, detectionRange, enemyLayer);

        float closestDistance = Mathf.Infinity;
        Transform closest = null;

        foreach (Collider2D hit in hits)
        {
            float distance = Vector2.Distance(transform.position, hit.transform.position);

            if (distance < closestDistance)
            {
                closestDistance = distance;
                closest = hit.transform;
            }
        }

        currentTarget = closest;
    }

    protected virtual bool IsTargetValid(Transform target)
    {
        return Vector2.Distance(transform.position, target.position) <= detectionRange;
    }

    protected abstract void Act();

    //This is a really long note, I mean really long. You won't believe how long it is. She sells sea shells on the sea shore, but the values of these shells will fall, due to the laws of supply and demand. No-one wants to buy shells when there's loads on the sand. Step one you must create a sense of scarcity. People will buy them if they think that they're rare you see. Stock pile them on an island until they're rarer than a diamond. Step two, you've got to make people think that they want them, really fuckin' want them, hit 'em like Bronson.
}