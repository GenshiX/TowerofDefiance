using UnityEngine;
using UnityEngine.InputSystem;

public class PurchaseClickTarget : MonoBehaviour
{
    [SerializeField] private TowerLevel towerLevel;
    [SerializeField] private PurchaseType purchaseType;

    private Camera mainCamera;
    private Collider2D clickCollider;

    private void Awake()
    {
        if (towerLevel == null)
        {
            towerLevel = FindFirstObjectByType<TowerLevel>();
        }

        mainCamera = Camera.main;
        clickCollider = GetComponent<Collider2D>();
    }

    private void Update()
    {
        if (Mouse.current == null)
            return;

        if (!Mouse.current.leftButton.wasPressedThisFrame)
            return;

        if (mainCamera == null)
        {
            mainCamera = Camera.main;
        }

        if (mainCamera == null || clickCollider == null)
            return;

        Vector2 mouseWorldPosition = mainCamera.ScreenToWorldPoint(
            Mouse.current.position.ReadValue()
        );

        if (!clickCollider.OverlapPoint(mouseWorldPosition))
            return;

        Debug.Log($"Clicked purchase target: {gameObject.name} -> {purchaseType}");

        if (towerLevel != null)
        {
            towerLevel.TryPurchase(purchaseType);
        }
    }
}