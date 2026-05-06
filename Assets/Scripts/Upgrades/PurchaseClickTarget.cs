using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PurchaseClickTarget : MonoBehaviour
{
    [SerializeField] private TowerLevel towerLevel;
    [SerializeField] private PurchaseType purchaseType;

    [Header("Visual Feedback")]
    [SerializeField] private Color availableColor = Color.white;
    [SerializeField] private Color purchasedColor = Color.green;
    [SerializeField] private Color failedClickColor = Color.red;
    [SerializeField] private float failedFlashDuration = 0.12f;

    private Camera mainCamera;
    private Collider2D clickCollider;
    private SpriteRenderer spriteRenderer;
    private bool visuallyPurchased;

    private void Awake()
    {
        if (towerLevel == null)
        {
            towerLevel = FindFirstObjectByType<TowerLevel>();
        }

        mainCamera = Camera.main;
        clickCollider = GetComponent<Collider2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Start()
    {
        RefreshVisual();
    }

    private void Update()
    {
        RefreshVisual();

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

        if (towerLevel == null)
            return;

        bool purchased = towerLevel.TryPurchase(purchaseType);

        if (purchased)
        {
            visuallyPurchased = true;
            SetColor(purchasedColor);
        }
        else if (!towerLevel.IsPurchaseComplete(purchaseType))
        {
            StartCoroutine(FlashFailedClick());
        }
    }

    private void RefreshVisual()
    {
        if (towerLevel == null || visuallyPurchased)
            return;

        if (towerLevel.IsPurchaseComplete(purchaseType))
        {
            visuallyPurchased = true;
            SetColor(purchasedColor);
        }
        else
        {
            SetColor(availableColor);
        }
    }

    private IEnumerator FlashFailedClick()
    {
        SetColor(failedClickColor);
        yield return new WaitForSeconds(failedFlashDuration);

        if (!visuallyPurchased)
        {
            SetColor(availableColor);
        }
    }

    private void SetColor(Color color)
    {
        if (spriteRenderer != null)
        {
            spriteRenderer.color = color;
        }
    }
}