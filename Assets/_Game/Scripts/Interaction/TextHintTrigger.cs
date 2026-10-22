using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

public class AreaTextTrigger : MonoBehaviour
{
    [SerializeField] private GameObject textBox;
    [SerializeField] private TMP_Text dialogueText;
    [SerializeField] private string message = "Press E";

    private bool playerInside;
    private bool dismissed;

    private void Awake()
    {
        textBox.SetActive(false);
    }

    private void Update()
    {
        if (!playerInside || dismissed) return;

        if (Keyboard.current != null &&
            Keyboard.current.eKey.wasPressedThisFrame)
        {
            dismissed = true;
            textBox.SetActive(false);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        playerInside = true;
        dismissed = false;
        dialogueText.text = message;
        textBox.SetActive(true);
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        playerInside = false;

        if (textBox != null)
            textBox.SetActive(false);
    }
}