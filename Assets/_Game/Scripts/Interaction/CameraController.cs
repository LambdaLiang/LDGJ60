using UnityEngine;

public class CameraController : MonoBehaviour
{
    [Header("Player")]
    public Transform player;

    [Header("Camera X Boundaries")]
    public float minX;
    public float maxX;

    private Camera cam;

    void Start()
    {
        cam = GetComponent<Camera>();
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
        float targetX = Mathf.Clamp(
            player.position.x,
            leftLimit,
            rightLimit
        );

        // Keep Y and Z unchanged
        transform.position = new Vector3(
            targetX,
            transform.position.y,
            transform.position.z
        );
    }
}
