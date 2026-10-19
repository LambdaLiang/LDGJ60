using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class EndingController : MonoBehaviour
{
    [Header("Trigger")]
    [SerializeField] private ClueProgress clueProgress;
    [SerializeField] private ClueDefinition triggerClue;

    [Header("Dialogue")]
    [SerializeField] private GameObject dialoguePanel;
    [SerializeField] private TMP_Text dialogueText;
    [TextArea(3, 10)]
    [SerializeField] private string[] dialoguePages = new string[0];

    [Header("Exploration")]
    [SerializeField] private PlayerMovement playerMovement;
    [SerializeField] private BookshelfInteraction[] bookshelves;

    private bool triggered;
    private bool playing;
    private int pageIndex;
    private int lastAdvanceFrame = -1;

    private bool previousPlayerBlocked;
    private bool[] previousShelfBlocked;
    private Coroutine finishRoutine;

    private void Awake()
    {
        if (clueProgress == null || triggerClue == null ||
            dialoguePanel == null || dialogueText == null ||
            playerMovement == null ||
            dialoguePages == null || dialoguePages.Length == 0)
        {
            Debug.LogError("Assign ending references and dialogue pages.", this);
            enabled = false;
            return;
        }

        dialoguePanel.SetActive(false);
    }

    private void OnEnable()
    {
        if (clueProgress != null)
            clueProgress.ClueCollected += OnClueCollected;
    }

    private void OnDisable()
    {
        if (clueProgress != null)
            clueProgress.ClueCollected -= OnClueCollected;

        if (finishRoutine != null)
        {
            StopCoroutine(finishRoutine);
            finishRoutine = null;
        }

        if (playing)
        {
            dialoguePanel.SetActive(false);
            RestoreInput();
            playing = false;
        }
    }

    private void OnClueCollected(ClueDefinition clue)
    {
        if (triggered || clue != triggerClue)
            return;

        triggered = true;
        playing = true;
        pageIndex = 0;
        lastAdvanceFrame = Time.frameCount;

        BlockInput();

        dialogueText.text = dialoguePages[pageIndex];
        dialoguePanel.SetActive(true);
    }

    private void Update()
    {
        Keyboard keyboard = Keyboard.current;

        if (keyboard != null && keyboard.eKey.wasPressedThisFrame)
            AdvanceDialogue();
    }

    public void AdvanceDialogue()
    {
        if (!playing || finishRoutine != null ||
            lastAdvanceFrame == Time.frameCount)
        {
            return;
        }

        lastAdvanceFrame = Time.frameCount;
        pageIndex++;

        if (pageIndex < dialoguePages.Length)
        {
            dialogueText.text = dialoguePages[pageIndex];
        }
        else
        {
            finishRoutine = StartCoroutine(FinishDialogue());
        }
    }

    private IEnumerator FinishDialogue()
    {
        dialoguePanel.SetActive(false);

        // 防止结束对白的同一次按键触发书架交互。
        yield return null;

        RestoreInput();
        playing = false;
        finishRoutine = null;
    }

    private void BlockInput()
    {
        previousPlayerBlocked = playerMovement.InputBlocked;
        playerMovement.InputBlocked = true;

        int count = bookshelves == null ? 0 : bookshelves.Length;
        previousShelfBlocked = new bool[count];

        for (int i = 0; i < count; i++)
        {
            if (bookshelves[i] == null)
                continue;

            previousShelfBlocked[i] = bookshelves[i].InputBlocked;
            bookshelves[i].InputBlocked = true;
        }
    }

    private void RestoreInput()
    {
        if (playerMovement != null)
            playerMovement.InputBlocked = previousPlayerBlocked;

        if (previousShelfBlocked == null)
            return;

        for (int i = 0; i < previousShelfBlocked.Length; i++)
        {
            if (bookshelves[i] != null)
                bookshelves[i].InputBlocked = previousShelfBlocked[i];
        }
    }
}
