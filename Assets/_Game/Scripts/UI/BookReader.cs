using UnityEngine;
using TMPro;

public class BookReader : MonoBehaviour
{
    [SerializeField] private GameObject bookPanel;
    [SerializeField] private TMP_Text titleText;
    [SerializeField] private TMP_Text contentText;
    [SerializeField] private ClueProgress clueProgress;

    [SerializeField] private AudioManager audioManager;

    private void Awake()
    {
        CloseBook();
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

        ClueDefinition clue = clueProgress.GetAvailableClue(book.book_clue_index);
        if (clue != null)
        {
            if (!string.IsNullOrWhiteSpace(book.clue_content))
            {
                contentText.text = book.clue_content;
                clueProgress.CollectClue(clue, book);
            }
            else
            {
                Debug.LogWarning($"Book {book.book_title} needs clue content.", book);
            }
        }


        bookPanel.SetActive(true);
        audioManager.PlayOpeningBook();
    }


    public void CloseBook()
    {
        if (bookPanel != null)
        {
            bookPanel.SetActive(false);
        }
        audioManager.PlayPuttingBookBack();
    }

    private void OnDisable()
    {
        CloseBook();
    }

}
