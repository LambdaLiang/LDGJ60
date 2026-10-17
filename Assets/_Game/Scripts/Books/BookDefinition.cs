using UnityEngine;

[CreateAssetMenu(
    fileName = "new_book",
    menuName = "Library Game/Book"
)]
public class BookDefinition : ScriptableObject
{
    // Book index number
    public int book_index;
    // The actual book title shown to player
    public string book_title;
    // TODO: change color in engine? on art asset?
    public Color book_color = Color.white;
    [Tooltip("-1 means this book has no associated clue.")]
    public int book_clue_index = -1;
    [TextArea(5, 15)]
    public string content;
}
