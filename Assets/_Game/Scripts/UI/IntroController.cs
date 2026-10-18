using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class IntroController : MonoBehaviour
{
    private enum IntroState
    {
        WaitingForStart,
        OpeningDialogue,
        FollowupDialogue,
        Finishing,
        Complete
    }

    [Header("Scene references")]
    [SerializeField] private GameObject startPanel;
    [SerializeField] private PlayerMovement playerMovement;
    [SerializeField] private BookshelfInteraction bookshelfInteraction;

    [Header("Dialogue")]
    [SerializeField] private GameObject dialoguePanel;
    [SerializeField] private TMP_Text dialogueText;

    [TextArea(3, 10)]
    [SerializeField] private string openingText;

    [TextArea(3, 10)]
    [SerializeField] private string followupText;

    [Header("Opening clue")]
    [SerializeField] private ClueProgress clueProgress;
    [SerializeField] private ClueDefinition openingClue;

    private IntroState state;
    private int lastAdvanceFrame = -1;

    private void Start()
    {
        if (startPanel == null ||
            playerMovement == null ||
            bookshelfInteraction == null ||
            dialoguePanel == null ||
            dialogueText == null ||
            clueProgress == null ||
            openingClue == null)
        {
            Debug.LogError("Assign all intro references.", this);
            enabled = false;
            return;
        }

        state = IntroState.WaitingForStart;

        startPanel.SetActive(true);
        dialoguePanel.SetActive(false);
        SetInputBlocked(true);
    }

    private void Update()
    {
        Keyboard keyboard = Keyboard.current;

        if (keyboard != null &&
            keyboard.eKey.wasPressedThisFrame)
        {
            AdvanceDialogue();
        }
    }

    public void BeginGame()
    {
        if (state != IntroState.WaitingForStart)
        {
            return;
        }

        state = IntroState.OpeningDialogue;
        lastAdvanceFrame = Time.frameCount;

        startPanel.SetActive(false);
        dialogueText.text = openingText;
        dialoguePanel.SetActive(true);
    }

    public void AdvanceDialogue()
    {
        if (state != IntroState.OpeningDialogue &&
            state != IntroState.FollowupDialogue)
        {
            return;
        }

        if (lastAdvanceFrame == Time.frameCount)
        {
            return;
        }

        lastAdvanceFrame = Time.frameCount;

        if (state == IntroState.OpeningDialogue)
        {
            if (clueProgress.GetAvailableClue(
                    openingClue.clue_index) != openingClue)
            {
                Debug.LogError(
                    "The opening clue must be registered " +
                    "and have no unmet prerequisites.",
                    this
                );

                return;
            }

            clueProgress.CollectClue(openingClue);

            state = IntroState.FollowupDialogue;
            dialogueText.text = followupText;
        }
        else
        {
            StartCoroutine(FinishIntro());
        }
    }

    private IEnumerator FinishIntro()
    {
        state = IntroState.Finishing;
        dialoguePanel.SetActive(false);

        // Keep the final E press from opening the shelf.
        yield return null;

        SetInputBlocked(false);
        state = IntroState.Complete;
    }

    private void SetInputBlocked(bool blocked)
    {
        if (playerMovement != null)
        {
            playerMovement.InputBlocked = blocked;
        }

        if (bookshelfInteraction != null)
        {
            bookshelfInteraction.InputBlocked = blocked;
        }
    }

    private void OnDisable()
    {
        StopAllCoroutines();
        SetInputBlocked(false);
    }
}