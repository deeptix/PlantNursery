using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

// Various health states a plant can have
public enum Health {
    Healthy, 
    Overwatered,
    Underwatered
}

// Various growth state a plant can have
public enum Growth {
    Sprout,      // Seedling
    Primary,     // First growth stage
    Secondary,   // Second growth stage
    Mature,      // Fully grown 
    Overgrown    // Needs trimming
}

public class PlantStateManager : MonoBehaviour
{

    private GameManager gameManager;
    public string plantName;        // Plant name
    public Plant requirements;      // Static care requirements for plant
    public int age;                 // Age of plant in days
    public Health healthState;      // Dynamic health state of plant
    public Growth growthState;      // Dynamic growth state of plant
    public float water;            // Amount of water plant has
    private float absorptionRate;  // per-day absorption rate
    // public SoilTypes soil;       // Type of soil plant is in
    // public Sunlight sun;         // Type of sun plant is in
    // public int temp;             // Temperature of plant environment

    // TESTING WATERING SYSTEM
    private bool watering;
    private float durationWaterTime;

    // Initialize plant
    void Start() {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();

        // Store plant care requirements 
        if (!gameManager.GetPlant(plantName, out requirements)) {
            Debug.Log("Could not find plant.");
        }
        // Plant starts as a healthy sprout
        age = 0;
        healthState = Health.Healthy;
        growthState = Growth.Sprout;

        // Plant starts off with the middle amount of water
        water = (requirements.minWater + requirements.maxWater) / 2;

        /* Absorption Rate:
        - calculated as (maxWater - minWater) / numDaysTillNextWater
        - each day, the plant will lose absorptionRate water
        - this means that if plants starts off with maxWater, then after wateringSchedule weeks,
          plant will have minWater left
        */
        absorptionRate = (requirements.maxWater - requirements.minWater)/(7*requirements.wateringSchedule);
    }

    // Updates Growth and Health states based on plant care + number of days skipped
    public void passTime(int numDays) {
        // Update water amount due to absorption by plant
        absorb(numDays);

        // Update health state based on water amount
        if (water < requirements.minWater) {
            healthState = Health.Underwatered;
        } else if (requirements.minWater <= water && water <= requirements.maxWater) {
            healthState = Health.Healthy;
        } else {
            healthState = Health.Overwatered;
        }
    }

    // Updates amount of water plant holds when user waters the plant
    // A plant can have a maximum of 10 water
    public void waterPlant(float addWaterAmount) {
        water = Math.Min(water + addWaterAmount, 10);
    }

    /* Functions to update plant states based on user actions

    public void movePlant(Sunlight newSun) {
        sun = newSun;
    }

    public void changeSoil(SoilTypes newSoil) {
        soil = newSoil;
    }

    public void changeTemperature(int newTemp) {
        temp = newTemp;
    }

    */

    /* Private Helper functions below */

    // After numDays days, water is absorbed based on the per-day evaporation raye
    private void absorb(int numDays) {
        water = Math.Min(absorptionRate * numDays, 0);
    }

    /* TESTING WATERING */
    void Update() {
        if (Input.GetMouseButton(0)) {
            waterPlant(Time.deltaTime);
        }
    }

    void OnGUI()
    {
        GUI.Label(new Rect(10, 10, 100, 20), water.ToString());
    }
}
