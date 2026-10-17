using UnityEngine;

[CreateAssetMenu(
    fileName = "game_audio",
    menuName = "Library Game/Audio")]
public class GameAudioDefinition : ScriptableObject
{
    [Header("Books")]
    public AudioClip changing_book;
    public AudioClip selecting_book;
    public AudioClip opening_book;
    public AudioClip closing_book;
    public AudioClip putting_book_back;

    [Header("Clues")]
    public AudioClip unlocking_clue;
}
