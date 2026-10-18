using UnityEngine;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

public class ClueProgress : MonoBehaviour
{
    [SerializeField] private ClueDefinition[] clues = new ClueDefinition[0];
    private readonly HashSet<int> collectedClues = new HashSet<int>();
    private readonly List<ClueDefinition> collectionHistory = new List<ClueDefinition>();
    public event Action<ClueDefinition> ClueCollected;
    public IReadOnlyList<ClueDefinition> CollectionHistory => collectionHistory;

    public ClueDefinition GetAvailableClue(int clueIndex)
    {
        if (clueIndex < 0)
        {
            return null;
        }

        foreach (ClueDefinition clue in clues)
        {
            if (clue == null || clue.clue_index != clueIndex)
            {
                continue;
            }
            if (clue.clue_type == ClueType.irrelevant_clue)
            {
                return null;
            }

            foreach (int prerequisite in clue.prerequisite_clue_indices)
            {
                if (!collectedClues.Contains(prerequisite))
                {
                    return null;
                }
            }
            return clue;
        }
        Debug.LogWarning($"No clue definition found for ID {clueIndex}", this);
        return null;
    }

    public void CollectClue(ClueDefinition clue)
    {
        if (clue == null || GetAvailableClue(clue.clue_index) != clue)
        {
            return;
        }

        if (collectedClues.Add(clue.clue_index))
        {
            // To record the order of finding clues
            collectionHistory.Add(clue);
            Debug.Log($"Collected clue: {clue.clue_title}");
            ClueCollected?.Invoke(clue);
        }
    }

    [ContextMenu("Check Duplicate Clue IDs")]
    private void CheckDuplicateClueIds()
    {
        var seen = new Dictionary<int, ClueDefinition>();
        bool foundDuplicate = false;

        foreach (ClueDefinition clue in clues)
        {
            if (clue == null)
            {
                continue;
            }

            if (seen.TryGetValue(clue.clue_index, out ClueDefinition first))
            {
                foundDuplicate = true;

                Debug.LogError(
                    $"Duplicate clue ID {clue.clue_index}: " +
                    $"{first.name} and {clue.name}",
                    clue
                );
            }
            else
            {
                seen.Add(clue.clue_index, clue);
            }
        }

        if (!foundDuplicate)
        {
            Debug.Log("No duplicate IDs in this clue list.", this);
        }
    }
}
