using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 4f;

    [Header("Character animation")]
    [SerializeField] private Animator characterAnimator;

    public bool InputBlocked { get; set; }

    private void Awake()
    {
        if (characterAnimator == null)
        {
            Debug.LogError("Assign the character Animator.", this);
            enabled = false;
        }
    }

    private void Update()
    {
        Keyboard keyboard = Keyboard.current;
        int direction = 0;

        if (!InputBlocked && keyboard != null)
        {
            if (keyboard.aKey.isPressed || keyboard.leftArrowKey.isPressed)
            {
                direction -= 1;
            }

            if (keyboard.dKey.isPressed || keyboard.rightArrowKey.isPressed)
            {
                direction += 1;
            }
        }

        characterAnimator.SetInteger("move_direction", direction);

        Vector3 position = transform.position;
        position.x += direction * moveSpeed * Time.deltaTime;
        transform.position = position;
    }

    private void OnDisable()
    {
        if (characterAnimator != null)
        {
            characterAnimator.SetInteger("move_direction", 0);
        }
    }
}