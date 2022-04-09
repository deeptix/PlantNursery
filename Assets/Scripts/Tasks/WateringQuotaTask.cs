using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class WateringQuotaTask : Task
{
    public PlantType plantType;
    public int quotaVal;

    private int currentVal = 0;

    public WateringQuotaTask(PlantType plantType, int quotaVal) 
    {
        this.plantType = plantType;
        this.quotaVal = quotaVal;

        EventBus.AddListener<Plant>(EventTypes.WateredPlant, UpdateCount);
    }

    private void UpdateCount(Plant plant) 
    {
        if (plant.plantType == plantType) {
            currentVal = Mathf.Min(currentVal + 1, quotaVal);
        }
    }

    public override string GetTaskName() 
    {
        return "Water plant of this type " + quotaVal + " times";
    }

    public override string GetTaskProgressString() 
    {
        return currentVal + "/" + quotaVal;
    }

    public override bool IsCompleted() 
    {
        return currentVal >= quotaVal;
    }
}
