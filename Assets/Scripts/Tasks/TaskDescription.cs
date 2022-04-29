using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum TaskType {
    WateringQuota,   // Fulfill task by watering plant x times

};

[System.Serializable]
public class TaskDescription
{
    public TaskType taskType;               // Type of the task
    public PlantType plantType;             // Type of the plant this applies to
    public string description;              // English description of the task
    public int goalValue;                   // Goal value for the task
}
