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
        AudioManager.Instance.PlayPhoneOpeningClosing();
    }

    public void CloseHistory()
    {
        historyPanel.SetActive(false);
        AudioManager.Instance.PlayPhoneOpeningClosing();
    }

    private int ComparePhoneOrder(
        ClueDefinition first,
        ClueDefinition second)
    {
        int comparison = first.phone_display_order.CompareTo(
            second.phone_display_order
        );

        if (comparison != 0)
        {
            return comparison;
        }

        // Equal display orders get a predictable tie-breaker.
        return first.clue_index.CompareTo(second.clue_index);
    }

    private void AppendClueBranch(
        ClueDefinition clue,
        int depth,
        List<ClueDefinition> sortedClues,
        HashSet<int> displayedIds,
        StringBuilder text)
    {
        if (!displayedIds.Add(clue.clue_index))
        {
            return;
        }

        int indentation = depth * 16;
        string heading = clueProgress.GetDisplayTitle(clue);

        text.Append($"<indent={indentation}>");
        if (depth == 0)
        {
            text.AppendLine("<color=#CE8787>New Story!</color>");
        }
        text.AppendLine($"<size=36><i><color=#738795>{heading}</color></i></size>");
        text.AppendLine(clue.phone_message);
        text.AppendLine("</indent>");
        text.AppendLine();

        foreach (ClueDefinition child in sortedClues)
        {
            if (child.phone_parent_clue_index == clue.clue_index)
            {
                AppendClueBranch(
                    child,
                    depth + 1,
                    sortedClues,
                    displayedIds,
                    text
                );
            }
        }
    }
    private void RefreshHistory()
    {
        List<ClueDefinition> sortedClues = new List<ClueDefinition>();

        foreach (ClueDefinition clue in clueProgress.CollectionHistory)
        {
            if (clue != null)
            {
                sortedClues.Add(clue);
            }
        }

        if (sortedClues.Count == 0)
        {
            historyText.text = "No clues collected yet.";
            return;
        }

        sortedClues.Sort(ComparePhoneOrder);

        HashSet<int> collectedIds = new HashSet<int>();

        foreach (ClueDefinition clue in sortedClues)
        {
            collectedIds.Add(clue.clue_index);
        }

        HashSet<int> displayedIds = new HashSet<int>();
        StringBuilder text = new StringBuilder();

        // Begin with roots, or clues whose display parent
        // has not been collected.
        foreach (ClueDefinition clue in sortedClues)
        {
            int parentId = clue.phone_parent_clue_index;

            if (parentId < 0 || !collectedIds.Contains(parentId))
            {
                AppendClueBranch(
                    clue,
                    0,
                    sortedClues,
                    displayedIds,
                    text
                );
            }
        }

        // Keep collected clues visible even if someone
        // accidentally creates a circular layout.
        foreach (ClueDefinition clue in sortedClues)
        {
            if (!displayedIds.Contains(clue.clue_index))
            {
                AppendClueBranch(
                    clue,
                    0,
                    sortedClues,
                    displayedIds,
                    text
                );
            }
        }

        historyText.text = text.ToString();
    }
}