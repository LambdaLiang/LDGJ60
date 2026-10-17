using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 4f;

    // Update is called once per frame
    void Update()
    {
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

        // Only move in horizontal
        Vector3 movement = new Vector3(direction, 0f, 0f);
        transform.position += movement * moveSpeed * Time.deltaTime;
    }
}
