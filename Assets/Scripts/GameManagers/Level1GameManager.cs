using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level1GameManager : GameManager
{
    private int numHealthyDays = 0;
    private int healthyDayQuota = 14;

    // Raising one scallion
    // Win condition: Reaches last growth state and is maintained for a month

    public override bool HasWon() {
        return numHealthyDays >= healthyDayQuota;
    }

    public override void UpdateStats(PlantStateManager pl) {
        foreach (PlantStateManager plant in plantManagers) {
            if (plant.growthState == Growth.Mature && plant.healthState == Health.Healthy) {
                numHealthyDays = plant.ageHealthy;
            }
        }
    }
}
