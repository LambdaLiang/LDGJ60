using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class BookshelfInteraction : MonoBehaviour
{
    [SerializeField] private Transform player;
    [SerializeField] private PlayerMovement playerMovement;
    [SerializeField] private GameObject bookshelfPanel;
    [SerializeField] private float interactionDistance = 2f;
    private bool playerWasNearby;
    private bool isOpen;
    public bool InputBlocked { get; set; }

    private void Awake()
    {
        if (player == null || playerMovement == null || bookshelfPanel == null)
        {
            Debug.LogError("Assign the player, movement component, and bookshelf panel.", this);
            enabled = false;
            return;
        }


        SetOpen(false);
    }
    // Update is called once per frame
    void Update()
    {
        if (InputBlocked)
        {
            return;
        }

        Keyboard keyboard = Keyboard.current;
        if (isOpen)
        {
            if (keyboard.escapeKey.wasPressedThisFrame)
            {
                CloseShelf();
            }
            return;
        }

        float playerShelfDistance = Vector2.Distance(transform.position, player.position);

        bool playerIsNearby = playerShelfDistance <= interactionDistance;

        if (playerIsNearby && !playerWasNearby)
        {
            Debug.Log("Press E to browse the bookshelf!");
        }

        playerWasNearby = playerIsNearby;

        if (playerIsNearby && keyboard.eKey.wasPressedThisFrame)
        {
            SetOpen(true);
        }
    }

    private void SetOpen(bool open)
    {
        isOpen = open;
        if (bookshelfPanel != null)
        {
            bookshelfPanel.SetActive(open);
        }

        if (playerMovement != null)
        {
            // If shelf open, player cannot move
            playerMovement.enabled = !open;
        }

    }

    public void CloseShelf()
    {
        SetOpen(false);
    }

    private void OnDisable()
    {
        if (isOpen)
        {
            SetOpen(false);
        }
    }
}
