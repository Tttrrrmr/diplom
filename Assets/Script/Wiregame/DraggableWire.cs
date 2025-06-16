using UnityEngine;
using UnityEngine.EventSystems;

public class DraggableWire : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    private RectTransform rectTransform;
    private CanvasGroup canvasGroup;
    private Transform originalParent;
    private int originalIndex;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        originalParent = transform.parent;
        originalIndex = transform.GetSiblingIndex();

        transform.SetParent(transform.root, false); // тащим поверх UI
        canvasGroup.blocksRaycasts = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        rectTransform.position = Input.mousePosition;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        canvasGroup.blocksRaycasts = true;

        // если не отпущен над DropZone Ч вернуть на исходную панель
        if (transform.parent == transform.root)
        {
            transform.SetParent(originalParent, false);
            transform.SetSiblingIndex(originalIndex);
        }
    }
}