using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropManager : MonoBehaviour
{
    public GameObject plantHolding;
    public Sunlight sun;
    [SerializeField] private GameObject dropzonePrefab;
    GameObject dropzoneArea;
    BoxCollider2D boxCollider2D;

    void Start()
    {
        EventBus.AddListener(EventTypes.DraggingPlant, new CallBack<bool, GameObject>(toggleCollider));

        dropzoneArea = Instantiate(dropzonePrefab);
        dropzoneArea.transform.SetParent(transform);
        dropzoneArea.transform.localPosition = Vector3.zero;
        boxCollider2D = GetComponent<BoxCollider2D>();

        DrawDropzoneBox();
        dropzoneArea.SetActive(false);
    }

    void DrawDropzoneBox()
    {
        Vector3 min = boxCollider2D.bounds.min;
        Vector3 max = boxCollider2D.bounds.max;
        dropzoneArea.transform.position = (min + max) / 2;
        float xResize = (max.x - min.x) * dropzoneArea.transform.localScale.x;
        float yResize = (max.y - min.y) * dropzoneArea.transform.localScale.y;

        dropzoneArea.transform.localScale = new Vector3(xResize, yResize, 1);
    }

    void toggleCollider(bool isDragging, GameObject plantDropped) 
    {
        dropzoneArea.SetActive(isDragging);
        if (!isDragging) {
            if (plantDropped == null) return;
            // if not holding a plant or was holding the plant that's being dragged, check if drop zone is colliding with the plant
            if (plantHolding == null || (plantHolding != null && plantHolding == plantDropped)) {
                if (gameObject.GetComponent<BoxCollider2D>().IsTouching(plantDropped.GetComponent<BoxCollider2D>())) {
                    // collided! update plant holding accordingly
                    plantHolding = plantDropped;
                    plantHolding.GetComponent<PlantStateManager>().movePlant(sun);
                } else {
                    // no collision --> set plant holding to null
                    plantHolding = null;
                }
            }
        }
    }
}
