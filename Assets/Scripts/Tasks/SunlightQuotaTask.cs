using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class SunlightQuotaTask : Task
{
    public PlantType plantType;
    public int quotaVal;

    private int currentVal = 0;

    private PlantStateManager[] plants;
    private GameManager gameManager;
    private Plant plantInfo;

    public SunlightQuotaTask(PlantType plantType, int quotaVal) 
    {
        this.plantType = plantType;
        this.quotaVal = quotaVal;


        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        gameManager.GetPlant(plantType, out plantInfo);

        plants = GameObject.FindObjectsOfType<PlantStateManager>();
        EventBus.AddListener<int>(EventTypes.DayPassed, UpdateCount);
    }

    private void UpdateCount(int numDays) 
    {
        foreach (PlantStateManager plant in plants) {
            if (plant.plantType == plantType && plant.sun == plantInfo.Sun) {
                currentVal = Mathf.Min(currentVal + numDays, quotaVal);
            }
        }
    }

    public override string GetTaskName() 
    {
        return "Give plant enough sunlight for " + quotaVal + " days";
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
