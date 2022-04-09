using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DragDropManager : MonoBehaviour, IPointerDownHandler, IBeginDragHandler, IEndDragHandler, IDragHandler
{
    [SerializeField] private Canvas canvas;

    private RectTransform rectTransform;
    private CanvasGroup canvasGroup;
    private int SIZE_ADJ_COLLIDER = 3;
    private int OFFSET_ADJ_COLLIDER = 1;

    private void Awake() {
        rectTransform = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();
    }

    public void adjustDropZoneBoxColliders(int sizeAdj, int offsetAdj) {
        GameObject[] dropZones = GameObject.FindGameObjectsWithTag("DropZone");

        foreach (GameObject dropZone in dropZones) {
            BoxCollider2D col = dropZone.GetComponent<BoxCollider2D>();
            col.size = new Vector2(col.size.x, col.size.y + sizeAdj);
            col.offset = new Vector2(col.offset.x, col.offset.y + offsetAdj);
        }
    }

    public void OnBeginDrag(PointerEventData eventData) {
        canvasGroup.blocksRaycasts = false;
        adjustDropZoneBoxColliders(SIZE_ADJ_COLLIDER, OFFSET_ADJ_COLLIDER);
    }

    public void OnDrag(PointerEventData eventData) {
        rectTransform.anchoredPosition += eventData.delta / canvas.scaleFactor;
    }

    public void OnEndDrag(PointerEventData eventData) {
        canvasGroup.blocksRaycasts = true;
        adjustDropZoneBoxColliders(-1 * SIZE_ADJ_COLLIDER, -1 * OFFSET_ADJ_COLLIDER);
    }

    public void OnPointerDown(PointerEventData eventData) {
    }

}
