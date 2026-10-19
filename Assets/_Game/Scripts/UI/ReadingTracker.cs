using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ReadingTracker : MonoBehaviour
{
    [SerializeField] private TMP_Text progressText;

    private readonly HashSet<BookDefinition> readBooks = new();

    public int ReadCount => readBooks.Count;

    private void Awake()
    {
        RefreshText();
    }

    public void MarkRead(BookDefinition book)
    {
        if (book == null || !readBooks.Add(book))
            return;
    }

    public void RefreshText()
    {
        if (progressText != null)
        {
            progressText.text = $"Books Read: {ReadCount}/20";
        }
    }
}