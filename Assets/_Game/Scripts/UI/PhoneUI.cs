using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic; // IReadOnlyList
using System.Text;                // StringBuilder

public class PhoneUI : MonoBehaviour
{
    [SerializeField] private ClueProgress clueProgress;

    [SerializeField] private Image phoneButtonImage;

    [SerializeField] private GameObject notificationPanel;
    [SerializeField] private TMP_Text notificationText;

    [SerializeField] private GameObject historyPanel;
    [SerializeField] private TMP_Text historyText;

    private readonly Color idleColor = Color.white;
    private readonly Color unreadColor =
        new Color(0.7f, 1f, 0.4f);

    private void Awake()
    {
        if (clueProgress == null ||
            phoneButtonImage == null ||
            notificationPanel == null ||
            notificationText == null ||
            historyPanel == null ||
            historyText == null)
        {
            Debug.LogError(
                "Assign all PhoneUI references.",
                this
            );

            enabled = false;
            return;
        }

        notificationPanel.SetActive(false);
        historyPanel.SetActive(false);
        phoneButtonImage.color = idleColor;
    }

    private void OnEnable()
    {
        if (clueProgress != null)
        {
            clueProgress.ClueCollected += OnClueCollected;
        }
    }

    private void OnDisable()
    {
        if (clueProgress != null)
        {
            clueProgress.ClueCollected -= OnClueCollected;
        }
    }

    private void OnClueCollected(ClueDefinition clue)
    {
        if (historyPanel.activeSelf)
        {
            RefreshHistory();
            return;
        }
        string heading = clueProgress.GetDisplayTitle(clue);

        notificationText.text =
            $"New clue: {heading}\nClick to read";

        notificationPanel.SetActive(true);
        phoneButtonImage.color = unreadColor;
    }

    public void OpenHistory()
    {
        RefreshHistory();

        historyPanel.SetActive(true);
        notificationPanel.SetActive(false);
        phoneButtonImage.color = idleColor;
    }

    public void CloseHistory()
    {
        historyPanel.SetActive(false);
    }

    private void RefreshHistory()
    {
        IReadOnlyList<ClueDefinition> history = clueProgress.CollectionHistory;

        if (history.Count == 0)
        {
            historyText.text = "No clues collected yet.";
            return;
        }

        StringBuilder text = new StringBuilder();

        for (int i = 0; i < history.Count; i++)
        {
            ClueDefinition clue = history[i];

            string heading = clueProgress.GetDisplayTitle(clue);
            text.AppendLine($"{i + 1}. {heading}");
            text.AppendLine(clue.phone_message);
            text.AppendLine();
        }

        historyText.text = text.ToString();
    }
}