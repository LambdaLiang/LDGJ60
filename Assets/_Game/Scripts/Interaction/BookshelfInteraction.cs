using UnityEngine;
using UnityEngine.InputSystem;

public class BookshelfInteraction : MonoBehaviour
{
    [SerializeField] private Transform player;
    [SerializeField] private PlayerMovement playerMovement;
    [SerializeField] private GameObject bookshelfPanel;
    [SerializeField] private BoxCollider2D interactionArea;

    private static BookshelfInteraction activeShelf;
    private bool playerWasNearby;
    private bool isOpen;

    public bool InputBlocked { get; set; }
    public bool IsOpen => isOpen;

    private void Awake()
    {
        if (interactionArea == null)
        {
            interactionArea = GetComponent<BoxCollider2D>();
        }

        if (player == null || playerMovement == null ||
            bookshelfPanel == null || interactionArea == null)
        {
            Debug.LogError("Assign the player, movement, panel, and interaction area.", this);
            enabled = false;
            return;
        }

        SetOpen(false);
    }

    private bool CanInteract()
    {
        return isActiveAndEnabled && !InputBlocked &&
            player != null && playerMovement != null &&
            !playerMovement.InputBlocked &&
            interactionArea != null && interactionArea.isActiveAndEnabled &&
            interactionArea.OverlapPoint(player.position);
    }

    private void Update()
    {
        if (InputBlocked) return;

        Keyboard keyboard = Keyboard.current;
        if (keyboard == null) return;

        if (isOpen)
        {
            if (keyboard.escapeKey.wasPressedThisFrame)
            {
                CloseShelf();
            }
            return;
        }

        bool nearby = CanInteract();
        if (nearby && !playerWasNearby)
        {
            Debug.Log("Press E to browse the bookshelf!", this);
        }
        playerWasNearby = nearby;

        if (nearby && keyboard.eKey.wasPressedThisFrame &&
            activeShelf == null && FindClosestShelf() == this)
        {
            SetOpen(true);
        }
    }

    private BookshelfInteraction FindClosestShelf()
    {
        BookshelfInteraction closest = null;
        float closestDistance = float.PositiveInfinity;

        // Only run on interaction attempts. Instance ID ordering breaks exact ties.
        foreach (BookshelfInteraction shelf in FindObjectsByType<BookshelfInteraction>(
            FindObjectsSortMode.InstanceID))
        {
            if (!shelf.CanInteract()) continue;

            float distance = Mathf.Abs(
                shelf.interactionArea.bounds.center.x - player.position.x);

            if (distance < closestDistance)
            {
                closestDistance = distance;
                closest = shelf;
            }
        }

        return closest;
    }

    private void SetOpen(bool open)
    {
        isOpen = open;
        if (open) activeShelf = this;
        else if (activeShelf == this) activeShelf = null;

        if (bookshelfPanel != null) bookshelfPanel.SetActive(open);
        if (playerMovement != null) playerMovement.enabled = !open;
    }

    public void CloseShelf()
    {
        SetOpen(false);
    }

    private void OnDisable()
    {
        if (isOpen) SetOpen(false);
    }
}
