using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;
using UnityEngine.UI;

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
    private RectTransform rectTransform; 
    private DrainageManager drainManager;
    public Sprite[] plantSprites;
    public TMP_Text stats;
    public PlantType plantType;     // Plant type
    public Plant requirements;      // Static care requirements for plant
    public int age;                 // Age of plant in days
    private int ageHealthy;         // Age of plant in a healthy state
    public Health healthState;      // Dynamic health state of plant
    public Growth growthState;      // Dynamic growth state of plant
    public float water;             // Amount of water plant has
    private float absorptionRate;   // per-day absorption rate
    public SoilTypes soil;          // Type of soil plant is in
    public Sunlight sun;            // Type of sun plant is in
    // public int temp;             // Temperature of plant environment
    private float idealWater;         // minWater + maxWater / 2

    void Awake() {
        plantSprites = Resources.LoadAll<Sprite>(plantType.ToString());
        // Debug.Log("Loaded " + plantSprites.Length + " sprites!");
        // foreach (Sprite plantSprite in plantSprites) {
        //     Debug.Log("Loaded " + plantSprite.name);
        // }
    }

    // Initialize plant
    void Start() {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        rectTransform = GetComponent<RectTransform>();
        drainManager = transform.Find("Pot").GetComponent<DrainageManager>();

        // Store plant care requirements 
        if (!gameManager.GetPlant(plantType, out requirements)) {
            Debug.Log("Could not find plant.");
        }
        // Plant starts as a healthy sprout with no soil
        age = 0;
        healthState = Health.Healthy;
        growthState = Growth.Sprout;

        // TODO: correctly initialize soil + sun based on given pot/positioning?
        // Currently initializes soil + sun to the plant care requirements
        soil = requirements.soil;
        sun = requirements.sun;

        // Plant starts off with the middle amount of water
        idealWater = (requirements.minWater + requirements.maxWater) / 2;
        water = idealWater;

        /* Absorption Rate:
        - calculated as (maxWater - minWater) / numDaysTillNextWater
        - each day, the plant will lose absorptionRate water
        - this means that if plants starts off with maxWater, then after wateringSchedule weeks,
          plant will have minWater left
        */
        absorptionRate = (requirements.maxWater - requirements.minWater)/(7*requirements.wateringSchedule);

        // Display initial stats
        displayStats();

        // Adding listeners
        EventBus.AddListener(EventTypes.DayPassed, new CallBack<int>(passTime));
    }

    // Updates Growth and Health states based on plant care + number of days skipped
    public void passTime(int numDays) {
        // Update overall plant age
        age += numDays;

        // Update water amount due to absorption by plant
        absorb(numDays);

        // Update health state based on water amount
        updateHealthState(numDays);

        // Update growth state based on number of days in a healthy state
        updateGrowthState();

        // Update sprite
        updateSprite();

        // Update displayed stats
        displayStats();
    }

    /* Functions to update plant states based on user actions */

    // Updates amount of water plant holds when user waters the plant
    // Note: A plant can have a maximum of 10 water
    // Note: addWaterAmount can be the time duration in which the user watered the plant for
    public void waterPlant(float addWaterAmount) {
        water = Math.Min(water + addWaterAmount, 10);
        if (water >= idealWater) {
            drainManager.turnDrainageOn();
        }
        displayStats();
    }

    // Updates sunlight provided when user moves the plant
    public void movePlant(Sunlight newSun) {
        sun = newSun;
    }

    // Updates the soil when user changes the soil
    public void changeSoil(SoilTypes newSoil) {
        soil = newSoil;
    }

    /* TODO: Add in temperature functionality afterwards 
    // Updates the temperature when user changes the temperature
    public void changeTemperature(int newTemp) {
        temp = newTemp;
    }
    */

    /* Private Helper functions below */
    
    // Returns true <==> external conditions (soil, sun) match plant requirements
    private bool correctExternalConditions() {
        return (soil == requirements.soil && sun == requirements.sun);
    }

    // Update health state based on water amount
    private void updateHealthState(int numDays) {
        if (water < requirements.minWater) {
            healthState = Health.Underwatered;
            ageHealthy = 0;
        } else if (requirements.minWater <= water && water <= requirements.maxWater) {
            // if plant was previously healthy and still healthy, update number of healthy days
            if (healthState == Health.Healthy) {
                ageHealthy += numDays;
            }
            healthState = Health.Healthy;
        } else {
            healthState = Health.Overwatered;
            ageHealthy = 0;
        }

        // if plant's external conditions are not correct, stop growth by setting ageHealthy to 0
        if (!correctExternalConditions()) {
            ageHealthy = 0;
        }
    }

    // Update growth state based on number of days in a healthy state according to growth schedule
    // If plant grows, reset healthy age to 0 for next stage
    private void updateGrowthState() {
        switch (growthState) {
            case Growth.Sprout:
                if (ageHealthy >= requirements.growthSchedule[0]) {
                    growthState = Growth.Primary;
                    ageHealthy = 0;
                }
                break;
            case Growth.Primary:
                if (ageHealthy >= requirements.growthSchedule[1]) {
                    growthState = Growth.Secondary;
                    ageHealthy = 0;
                }
                break;
            case Growth.Secondary:
                if (ageHealthy >= requirements.growthSchedule[2]) {
                    growthState = Growth.Mature;
                    ageHealthy = 0;
                }
                break;
            case Growth.Mature:
                if (ageHealthy >= requirements.growthSchedule[3]) {
                    growthState = Growth.Overgrown;
                    ageHealthy = 0;
                }
                break;
            default:
                break;
        }
    }

    // After numDays days, water is absorbed based on the per-day evaporation raye
    private void absorb(int numDays) {
        water = Math.Max(water - (absorptionRate * numDays), 0);
    }

    // Formats the plant stats into a string
    private string formatStats() {
        return "Plant Name: "   + plantType.ToString()   + "\n"
             + "Age: "          + age.ToString()         + "\n"
             + "Healthy Age: "  + ageHealthy.ToString()  + "\n"
             + "Health State: " + healthState.ToString() + "\n"
             + "Growth State: " + growthState.ToString() + "\n"
             + "Water: "        + water.ToString()       + "\n"
             + "Soil: "         + soil.ToString()        + "\n"
             + "Sun: "          + sun.ToString()         + "\n";
    }

    private void displayStats() {
        stats.text = formatStats();
    }

    private string getSpriteName() {
        string spriteName = plantType.ToString().ToLower() + "_";
        string lowerHealthState = healthState.ToString().ToLower();
        switch (growthState) {
            case Growth.Sprout:
                spriteName += "sprout";
                break;
            case Growth.Primary:
                spriteName += "growth 1 " + lowerHealthState;
                break;
            case Growth.Secondary:
                spriteName += "growth 2 " + lowerHealthState;
                break;
            case Growth.Mature:
                spriteName += "growth 3 " + lowerHealthState;
                break;
            case Growth.Overgrown:
                spriteName += "overgrown";
                break;
        }
        return spriteName;
    }

    private Sprite getSprite(string spriteName) {
        foreach (Sprite plantSprite in plantSprites) {
            if (plantSprite.name == spriteName) {
                return plantSprite;
            }
        }
        Debug.Log("Could not find sprite " + spriteName);
        return null;
    }

    private void updateSprite() {
        string spriteName = getSpriteName();
        string childSpriteName = plantType.ToString() + "Plant";
        transform.Find(childSpriteName).GetComponent<SpriteRenderer>().sprite = getSprite(spriteName);
    }

    /* TESTING WATERING */
    
    // Called every frame while the mouse stays over this object
    // private void OnMouseOver() {
    //     if (Input.GetMouseButton(0)) {
    //         waterPlant(Time.deltaTime);
    //         displayStats();
    //     }
    // }


}
