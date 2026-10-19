using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 4f;

    [Header("Character visual")]
    [SerializeField] private SpriteRenderer characterSprite;
    [SerializeField] private bool spriteFacesRight = true;

    /*
    [Header("Movement boundaries")]
    [SerializeField] private SpriteRenderer backgroundSprite;
    [SerializeField, Min(0f)] private float edgePadding = 0.1f;
    */
    public bool InputBlocked { get; set; }

    private void Awake()
    {
        if (characterSprite == null)
        {
            Debug.LogError("Assign the character SpriteRenderer.", this);
            enabled = false;
        }
    }
    // Update is called once per frame
    void Update()
    {
        if (InputBlocked)
        {
            return;
        }
        Keyboard keyboard = Keyboard.current;

        float direction = 0f;

        if (keyboard.aKey.isPressed || keyboard.leftArrowKey.isPressed)
        {
            direction -= 1f;
        }

        if (keyboard.dKey.isPressed || keyboard.rightArrowKey.isPressed)
        {
            direction += 1f;
        }

        if (direction != 0f)
        {
            bool movingRight = direction > 0f;
            characterSprite.flipX = movingRight != spriteFacesRight;
        }

        // Only move in horizontal
        Vector3 position = transform.position;
        position.x += direction * moveSpeed * Time.deltaTime;
        // Clamp to background
        transform.position = position;
    }
}
