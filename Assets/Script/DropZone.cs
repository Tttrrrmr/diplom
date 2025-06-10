using UnityEngine;
using UnityEngine.EventSystems;

public class DropZone : MonoBehaviour, IDropHandler
{
    public void OnDrop(PointerEventData eventData)
    {
        Debug.Log("OnDrop!");

        if (eventData.pointerDrag != null)
        {
            eventData.pointerDrag.transform.SetParent(transform, false);
        }
    }
}