using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonHover : MonoBehaviour,IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] 
    private GameObject hoverContent;

    private void Start()
    {
        SetVisible(false);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        SetVisible(true);
        AudioManager.Instance.PlaySelectingBook();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        SetVisible(false);
    }

    private void OnDisable()
    {
        SetVisible(false);
    }

    private void SetVisible(bool visible)
    {
        if (hoverContent != null)
            hoverContent.SetActive(visible);
    }
}
