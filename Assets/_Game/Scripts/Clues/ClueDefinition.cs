using UnityEngine;

public enum ClueType
{
    main_clue = 0,
    side_clue_starting = 1,
    side_clue = 2,
    irrelevant_clue = 3,
    easter_egg_clue = 4
}

[CreateAssetMenu(
    fileName = "new_clue",
    menuName = "Library Game/Clue")]
public class ClueDefinition : ScriptableObject
{
    [Min(0)]
    public int clue_index;

    public ClueType clue_type;

    // public string clue_title;

    [TextArea(5, 15)]
    public string phone_message;

    [Tooltip("All listed clues must be recognized first. Leave empty for no prerequisites.")]
    public int[] prerequisite_clue_indices = new int[0];

    [Tooltip("Designer-facing code, such as M1 or S1.")]
    public string clue_code;
}
