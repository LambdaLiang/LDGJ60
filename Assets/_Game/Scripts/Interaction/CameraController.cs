using UnityEngine;

public class CameraController : MonoBehaviour
{
    [Header("Player")]
    public Transform player;

    [Header("Camera X Boundaries")]
    public float minX;
    public float maxX;

    private Camera cam;

    void Awake()
    {
        cam = GetComponent<Camera>();

        if (cam == null || !cam.orthographic)
        {
            Debug.LogError("CameraController requires an orthographic Camera.", this);
            enabled = false;
            return;
        }

        // Follow in world space without inheriting the player's movement.
        transform.SetParent(null, true);
    }

    void LateUpdate()
    {
        if (player == null) return;

        // Calculate half of camera width
        float halfWidth = cam.orthographicSize * cam.aspect;

        // Calculate camera center boundaries
        float leftLimit = minX + halfWidth;
        float rightLimit = maxX - halfWidth;

        // Follow player on X axis only
        // Hold at an edge until the player crosses the camera center again.
        // If the viewport is wider than the level, keep it centered on the level.
        float targetX = leftLimit <= rightLimit
            ? Mathf.Clamp(player.position.x, leftLimit, rightLimit)
            : (minX + maxX) * 0.5f;

        // Keep Y and Z unchanged
        transform.position = new Vector3(
            targetX,
            transform.position.y,
            transform.position.z
        );
    }
}
