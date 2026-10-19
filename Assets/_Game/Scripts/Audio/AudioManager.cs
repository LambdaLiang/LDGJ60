using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [SerializeField] private GameAudioDefinition audioDefinition;
    [SerializeField] private AudioSource musicSource;
    [SerializeField] private AudioSource sfxSource;

    private void PlaySfx(AudioClip clip)
    {
        if (sfxSource != null && clip != null)
        {
            sfxSource.PlayOneShot(clip);
        }
    }

    public void PlayChangingBook()
    {
        if (audioDefinition != null)
            PlaySfx(audioDefinition.changing_book);
    }

    public void PlaySelectingBook()
    {
        if (audioDefinition != null)
            PlaySfx(audioDefinition.selecting_book);
    }

    public void PlayOpeningBook()
    {
        if (audioDefinition != null)
            PlaySfx(audioDefinition.opening_book);
    }

    public void PlayClosingBook()
    {
        if (audioDefinition != null)
            PlaySfx(audioDefinition.closing_book);
    }

    public void PlayPuttingBookBack()
    {
        if (audioDefinition != null)
            PlaySfx(audioDefinition.putting_book_back);
    }

    public void PlayUnlockingClue()
    {
        if (audioDefinition != null)
            PlaySfx(audioDefinition.unlocking_clue);
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
