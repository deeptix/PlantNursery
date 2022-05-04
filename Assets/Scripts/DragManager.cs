using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragManager : MonoBehaviour
{
    private int ADJ_POSITION = 1;
    private bool isDragging;
    private bool isMouseDown;
    private Vector2 oldPosition;

    public void OnMouseDown()
    {
        Debug.Log("Plant clicked!");
        if (!GetComponent<Zoom>().IsZoomed) {
            isMouseDown = true;
            Invoke(nameof(StartDragging), 0.5f);        
        }
    }

    void StartDragging() {
        if (isMouseDown && !GetComponent<Zoom>().IsZoomed) {
            isDragging = true;
            EventBus.Broadcast<bool, GameObject>(EventTypes.DraggingPlant, true, gameObject);
            oldPosition = transform.position;
        }
    }

    public void OnMouseUp()
    {
        isMouseDown = false;

        if (isDragging) {
            LayerMask mask = LayerMask.GetMask("DropZones");
            RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.zero, Mathf.Infinity, mask);

            // check if plant hit a drop zone and does not overlap with another plant
            if (hit.collider != null)
            {
                DropManager dropZone = hit.transform.gameObject.GetComponent<DropManager>();
                if (dropZone.plantHolding == null) {
                    // hit an empty drop zone! change positioning to match drop zone
                    RectTransform dropZoneRect = hit.transform.gameObject.GetComponent<RectTransform>();
                    Vector3 newPosition = new Vector3(dropZoneRect.transform.position.x, dropZoneRect.transform.position.y + ADJ_POSITION, -1);
                    transform.position = newPosition;

                    // have drop zones update the plants that they're holding
                    EventBus.Broadcast<bool, GameObject>(EventTypes.DraggingPlant, false, gameObject);
                } else {
                    // drop zone already had a plant, go back to original starting place
                    EventBus.Broadcast<bool, GameObject>(EventTypes.DraggingPlant, false, null);
                    transform.position = oldPosition;
                }
            } else {
                // did not land in a drop zone, go back to original starting place
                EventBus.Broadcast<bool, GameObject>(EventTypes.DraggingPlant, false, null);
                transform.position = oldPosition;
            }
            isDragging = false;
        }
    }

    void Update()
    {
        if (isDragging) {
            Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
            transform.Translate(mousePosition);
        }
    }
}
