using UnityEngine;
using UnityEngine.EventSystems;

public class HoverExpand : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public float expandAmount = 40f;   // How many pixels to expand
    public float speed = 8f;           // How fast it expands

    private RectTransform rectTransform;
    private float originalWidth;
    private float targetWidth;

    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        originalWidth = rectTransform.sizeDelta.x;
        targetWidth = originalWidth;
    }

    void Update()
    {
        float newWidth = Mathf.Lerp(
            rectTransform.sizeDelta.x,
            targetWidth,
            Time.deltaTime * speed
        );

        rectTransform.sizeDelta = new Vector2(newWidth, rectTransform.sizeDelta.y);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        targetWidth = originalWidth + expandAmount;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        targetWidth = originalWidth;
    }
}