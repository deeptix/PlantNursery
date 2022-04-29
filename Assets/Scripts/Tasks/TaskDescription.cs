using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


public enum TaskType {
    WateringQuota,   // Fulfill task by watering plant x times
    SoilQuota,       // Fulfill task by having plant grow in correct soil for x days
    SunlightQuota,   // Fulfill task by having plant grow in correct sunlight amount for x days
};

[System.Serializable]
public class TaskDescription
{
    public string taskTypeName;             // Type of the task
    public int goalValue;                   // Goal value for the task
    
    public TaskType taskType {
        get {
            return (TaskType)Enum.Parse(typeof(TaskType), taskTypeName);
        }
    }

    public Task task;

    public void SetPlantType(PlantType plantType) {
        switch (taskType) {
            case TaskType.SoilQuota:
                task = new SoilQuotaTask(plantType, goalValue);
                break;
            case TaskType.SunlightQuota:
                task = new SunlightQuotaTask(plantType, goalValue);
                break;
            case TaskType.WateringQuota:
                task = new WateringQuotaTask(plantType, goalValue);
                break;
            default:
                break;
        }
    }
}
