using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ItemDropHandler : MonoBehaviour, IDropHandler
{
    private int ADJ_POSITION = 80;

    public void OnDrop(PointerEventData eventData)
    {
        if (eventData.pointerDrag != null)
        {
            RectTransform dropZoneRect = GetComponent<RectTransform>();
            Vector2 newAnchoredPosition = new Vector2(dropZoneRect.anchoredPosition.x, dropZoneRect.anchoredPosition.y + ADJ_POSITION);
            eventData.pointerDrag.GetComponent<RectTransform>().anchoredPosition = newAnchoredPosition;
        }
    }
}
