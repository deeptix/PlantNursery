using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level2GameManager : GameManager
{
    private int[] numHealthyDays;
    private int[] healthyDayQuota;

    void Start() {
        numHealthyDays = new int[plantManagers.Length];
        healthyDayQuota = new int[plantManagers.Length];
        for (int i = 0; i < healthyDayQuota.Length; i++) {
            healthyDayQuota[i] = 14;
        }
    }

    // Raising one scallion and one peace lily
    // Win condition: Both reach last growth state and is maintained for a month

    public override bool HasWon() {
        for (int i = 0; i < healthyDayQuota.Length; i++) {
            if (numHealthyDays[i] < healthyDayQuota[i]) return false;
        }
        return true;
    }

    public override void UpdateStats(PlantStateManager pl) {
        for (int i = 0; i < plantManagers.Length; i++) {
            PlantStateManager plant = plantManagers[i];
            if (plant.growthState == Growth.Mature && plant.healthState == Health.Healthy) {
                numHealthyDays[i] = plant.ageHealthy;
            }
        }
    }
}
