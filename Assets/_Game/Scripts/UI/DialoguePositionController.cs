using UnityEngine;

public class DialogueBubbleFollow : MonoBehaviour
{
    [SerializeField] private Transform target;
    [SerializeField] private Camera worldCamera;
    [SerializeField] private Canvas canvas;

    private RectTransform bubble;
    private RectTransform parentRect;

    private void Awake()
    {
        bubble = GetComponent<RectTransform>();
        parentRect = transform.parent as RectTransform;

        if (target == null || worldCamera == null ||
            canvas == null || bubble == null || parentRect == null)
        {
            Debug.LogError("Assign dialogue bubble follow references.", this);
            enabled = false;
        }
    }

    private void LateUpdate()
    {
        Vector3 screenPosition =
            worldCamera.WorldToScreenPoint(target.position);

        Camera uiCamera =
            canvas.renderMode == RenderMode.ScreenSpaceOverlay
                ? null
                : canvas.worldCamera;

        if (RectTransformUtility.ScreenPointToWorldPointInRectangle(
                parentRect,
                screenPosition,
                uiCamera,
                out Vector3 uiPosition))
        {
            bubble.position = uiPosition;
        }
    }
}
