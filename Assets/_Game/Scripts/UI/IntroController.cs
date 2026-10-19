using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class IntroController : MonoBehaviour
{
    private enum IntroState
    {
        WaitingForStart,
        Dialogue,
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
    [SerializeField] private string[] dialoguePages = new string[0];

    [Header("Opening clue")]
    [SerializeField] private ClueProgress clueProgress;
    [SerializeField] private ClueDefinition openingClue;
    [Tooltip("Page number, starting at 1. Award M1 when this page is dismissed.")]
    [SerializeField, Min(1)] private int collectClueAfterPage = 3;

    [Header("Phone visibility")]
    [SerializeField] private CanvasGroup phoneGroup;

    private IntroState state;
    private int pageIndex;
    private int lastAdvanceFrame = -1;

    private void Start()
    {
        if (startPanel == null || playerMovement == null ||
            bookshelfInteraction == null || dialoguePanel == null ||
            dialogueText == null || clueProgress == null ||
            openingClue == null || phoneGroup == null)
        {
            Debug.LogError("Assign all intro references.", this);
            enabled = false;
            return;
        }

        if (dialoguePages == null || dialoguePages.Length == 0 ||
            collectClueAfterPage < 1 || collectClueAfterPage > dialoguePages.Length)
        {
            Debug.LogError("Add dialogue pages and choose a valid clue-award page.", this);
            enabled = false;
            return;
        }

        state = IntroState.WaitingForStart;
        startPanel.SetActive(true);
        dialoguePanel.SetActive(false);
        SetInputBlocked(true);
        SetPhoneVisible(false);
    }

    private void Update()
    {
        Keyboard keyboard = Keyboard.current;
        if (keyboard != null && keyboard.eKey.wasPressedThisFrame)
        {
            AdvanceDialogue();
        }
    }

    public void BeginGame()
    {
        if (state != IntroState.WaitingForStart) return;

        state = IntroState.Dialogue;
        pageIndex = 0;
        lastAdvanceFrame = Time.frameCount;
        startPanel.SetActive(false);
        dialogueText.text = dialoguePages[pageIndex];
        dialoguePanel.SetActive(true);
    }

    public void AdvanceDialogue()
    {
        if (state != IntroState.Dialogue || lastAdvanceFrame == Time.frameCount) return;
        lastAdvanceFrame = Time.frameCount;

        if (pageIndex + 1 == collectClueAfterPage)
        {
            if (clueProgress.GetAvailableClue(openingClue.clue_index) != openingClue)
            {
                Debug.LogError("Register the opening clue and check its prerequisites.", this);
                return;
            }

            clueProgress.CollectClue(openingClue);
        }

        pageIndex++;
        if (pageIndex < dialoguePages.Length)
        {
            dialogueText.text = dialoguePages[pageIndex];
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
        yield return null;

        SetPhoneVisible(true);
        SetInputBlocked(false);
        state = IntroState.Complete;
    }

    private void SetPhoneVisible(bool visible)
    {
        if (phoneGroup == null) return;

        phoneGroup.alpha = visible ? 1f : 0f;
        phoneGroup.interactable = visible;
        phoneGroup.blocksRaycasts = visible;
    }

    private void SetInputBlocked(bool blocked)
    {
        if (playerMovement != null) playerMovement.InputBlocked = blocked;
        if (bookshelfInteraction != null) bookshelfInteraction.InputBlocked = blocked;
    }

    private void OnDisable()
    {
        StopAllCoroutines();
        SetInputBlocked(false);
        SetPhoneVisible(true);
    }
}
