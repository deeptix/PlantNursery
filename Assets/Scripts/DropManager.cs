using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropManager : MonoBehaviour
{
    public GameObject plantHolding;
    public Sunlight sun;

    // Start is called before the first frame update
    void Start()
    {
        EventBus.AddListener(EventTypes.DraggingPlant, new CallBack<bool, GameObject>(toggleCollider));
    }

    void toggleCollider(bool isDragging, GameObject plantDropped) 
    {
        if (!isDragging) {
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
