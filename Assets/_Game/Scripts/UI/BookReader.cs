using UnityEngine;
using TMPro;

public class BookReader : MonoBehaviour
{
    public event System.Action BookClosed;

    [SerializeField] private GameObject bookPanel;
    [SerializeField] private TMP_Text titleText;
    [SerializeField] private TMP_Text contentText;
    [SerializeField] private ClueProgress clueProgress;

    [SerializeField] private AudioManager audioManager;

    [SerializeField] private ReadingTracker readingTracker;

    private void Awake()
    {
        if (bookPanel != null)
        {
            bookPanel.SetActive(false);
        }
    }

    public void OpenBook(BookDefinition book)
    {
        if (book == null || bookPanel == null || titleText == null ||
            contentText == null || clueProgress == null)
        {
            Debug.LogError("Assign book, reader panel, text reference, or clue progress", this);
            return;
        }

        titleText.text = book.book_title;

        // Ordinary content (without clue)
        contentText.text = book.content;
        bool countsAsRead = book.book_clue_index == -1;

        ClueDefinition clue = clueProgress.GetAvailableClue(book.book_clue_index);
        if (clue != null)
        {
            if (!string.IsNullOrWhiteSpace(book.clue_content))
            {
                contentText.text = book.clue_content;
                clueProgress.CollectClue(clue, book);
                countsAsRead = true;
            }
            else
            {
                Debug.LogWarning($"Book {book.book_title} needs clue content.", book);
            }
        }


        bookPanel.SetActive(true);
        if (countsAsRead && readingTracker != null)
        {
            readingTracker.MarkRead(book);
        }
        audioManager.PlayOpeningBook();
    }


    public void CloseBook()
    {
        if (bookPanel == null || !bookPanel.activeSelf)
            return;

        bookPanel.SetActive(false);

        if (audioManager != null)
            audioManager.PlayPuttingBookBack();

        BookClosed?.Invoke();
    }

    private void OnDisable()
    {
        if(bookPanel != null)
        {
            bookPanel.SetActive(false);
        }
    }

}
