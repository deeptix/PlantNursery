using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;
using UnityEngine.UI;

// Various health states a plant can have
public enum Health
{
    Healthy,
    Overwatered,
    Underwatered
}

// Various growth state a plant can have
public enum Growth
{
    Sprout,      // Seedling
    Primary,     // First growth stage
    Secondary,   // Second growth stage
    Mature,      // Fully grown
    Overgrown    // Needs trimming
}

public class PlantStateManager : MonoBehaviour
{

    private GameManager gameManager;
    private SoilChangeManager soilChangeManager;
    private RectTransform rectTransform;
    private DrainageManager drainManager;
    private SpriteRenderer spriteRenderer;
    private SpriteRenderer soilRenderer;
    public Sprite[] plantSprites;
    public TMP_Text stats;
    public PlantType plantType;     // Plant type
    public Plant requirements;      // Static care requirements for plant
    public int age;                 // Age of plant in days
    public int ageHealthy;          // Age of plant in a healthy state
    public Health healthState;      // Dynamic health state of plant
    public Growth growthState;      // Dynamic growth state of plant
    public float water;             // Amount of water plant has
    private float absorptionRate;   // per-day absorption rate
    public SoilTypes soil;          // Type of soil plant is in
    public Sunlight sun;            // Type of sun plant is in
    // public int temp;             // Temperature of plant environment
    private float idealWater;         // minWater + maxWater / 2

    public float SUNLIGHT_ADJ = 0.1f; // Absorption adjustment for incorrect sunlight
    public float SOIL_ADJ = 0.1f;     // Absorption adjustment for incorrect soil
    public float RED_ADJ = 0.2f;
    public float GREEN_ADJ = 0.1f;
    public float BLUE_ADJ = 0.1f;
    public float LOWEST_RED_COLOR = 0.3f;
    public float LOWEST_GREEN_COLOR = 0.1f;
    public float LOWEST_BLUE_COLOR = 0.1f;

    public GameObject waterMeter;

    private const float MAX_WATER_AMT = 10;

    void Awake()
    {
        plantSprites = Resources.LoadAll<Sprite>(plantType.ToString());
    }

    // Initialize plant
    void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        soilChangeManager = GameObject.Find("SoilManager").GetComponent<SoilChangeManager>();
        rectTransform = GetComponent<RectTransform>();
        drainManager = transform.Find("Pot").GetComponent<DrainageManager>();

        string childSpriteName = plantType.ToString() + "Plant";
        spriteRenderer = transform.Find(childSpriteName).GetComponent<SpriteRenderer>();
        soilRenderer = transform.Find("Pot").transform.Find("Pot Top").GetComponent<SpriteRenderer>();

        // Store plant care requirements
        if (!gameManager.GetPlant(plantType, out requirements))
        {
            Debug.Log("Could not find plant.");
        }

        // Plant starts off with the minimum acceptable amount of water
        //      --> forces watering on the first day
        idealWater = (requirements.minWater + requirements.maxWater) / 2;
        water = requirements.minWater;

        // just for debugging purposes --> updating here is unnecessary (only needs to be done in passTime)
        updateAbsorptionRate();

        // Display initial stats
        displayStats();

        // Initialize color of sprite based on ageHealthy
        updateHealthyColor(ageHealthy, 1);

        // Adding listeners
        EventBus.AddListener(EventTypes.DayPassed, new CallBack<int>(passTime));
        EventBus.AddListener(EventTypes.FinishedLevel, new CallBack(RemoveAllListeners));
    }

    void RemoveAllListeners()
    {
        EventBus.RemoveListener<int>(EventTypes.DayPassed, passTime);
        EventBus.RemoveListener(EventTypes.FinishedLevel, RemoveAllListeners);
    }

    // Updates Growth and Health states based on plant care + number of days skipped
    public void passTime(int numDays)
    {
        // Update overall plant age
        age += numDays;

        // Update absorption rate based on external conditions
        updateAbsorptionRate();

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

        EventBus.Broadcast<PlantStateManager>(EventTypes.UpdatedPlant, this);

        // Stop and clear any drainage as time is passing
        drainManager.clearDrainage();
    }

    /* Functions to update plant states based on user actions */

    // Updates amount of water plant holds when user waters the plant
    // Note: A plant can have a maximum of 10 water
    // Note: addWaterAmount can be the time duration in which the user watered the plant for
    public void waterPlant(float addWaterAmount) {
        water = Math.Min(water + addWaterAmount, MAX_WATER_AMT);
        if (water >= 0.95 * requirements.maxWater) {
            drainManager.turnDrainageOn();
        }
        displayStats();
    }

    public void movePlant(Sunlight newSun) {
        sun = newSun;

        updateAbsorptionRate();
        displayStats();
    }

    // Updates the soil when user changes the soil
    public void changeSoil(SoilTypes newSoil) {
        soil = newSoil;
        soilRenderer.color = soilChangeManager.GetSoilColor(soil);

        updateAbsorptionRate();
        displayStats();
    }

    /* Potential feature: temperature functionality
    // Updates the temperature when user changes the temperature
    public void changeTemperature(int newTemp) {
        temp = newTemp;
    }
    */

    /* Private Helper functions below */

    /* Absorption Rate:
    - if external conditions are correct, calculated as (maxWater - minWater) / numDaysTillNextWater
    - affected by incorrect sunlight/soil
    - each day, the plant will lose absorptionRate water
    - this means that if plants starts off with maxWater, then after wateringSchedule weeks,
        plant will have minWater left
    */
    private void updateAbsorptionRate() {
        float numerator = requirements.maxWater - requirements.minWater;

        // Too much sun --> increase absorption rate
        // Too little sun --> decrease absorption rate
        // (small sunlight value, high retention rate <--> large sunlight value, low retention rate)
        numerator += SUNLIGHT_ADJ * (sun - requirements.Sun);

        // Soil retains too much water --> decrease absorption rate
        // Soil does not retain enough water --> increase absorption rate
        // (small soil value, high retention rate <--> large soil value, low retention rate)
        numerator += SOIL_ADJ * (soil - requirements.Soil);

        absorptionRate = numerator / (7 * requirements.wateringSchedule);
    }

    // Returns true <==> external conditions (soil, sun) match plant requirements
    private bool correctExternalConditions()
    {
        return (soil == requirements.Soil);
    }

    // Clamps color components (r,g,b,a) in between LOWEST_<color>_COLOR and 1 inclusive
    private float clampColor(float newColorComp, String color) {
        switch (color) {
            case "RED":
                return Mathf.Clamp(newColorComp, LOWEST_RED_COLOR, 1);
            case "GREEN":
                return Mathf.Clamp(newColorComp, LOWEST_GREEN_COLOR, 1);
            case "BLUE":
                return Mathf.Clamp(newColorComp, LOWEST_BLUE_COLOR, 1);
            default:
                return Mathf.Clamp(newColorComp, 0, 1);
        }
    }

    // Updates color overlay of plant sprite
    // If sign == 1, color recovers to the original coloring (trying to become healthy)
    // If sign == -1, color slowly changes to black (becoming unhealthy)
    private void updateHealthyColor(int numDays, int sign) {
        int multFactor = numDays * sign;

        Color currColor = spriteRenderer.color;
        if (currColor != Color.white || healthState != Health.Healthy) {
            float red = clampColor(currColor.r + multFactor * RED_ADJ, "RED");
            float green = clampColor(currColor.g + multFactor * GREEN_ADJ, "GREEN");
            float blue = clampColor(currColor.b + multFactor * BLUE_ADJ, "BLUE");
            spriteRenderer.color = new Color(red, green, blue, currColor.a);
            Debug.Log("New color: " + spriteRenderer.color.ToString());
        }
    }

    // Update health state based on water amount
    private void updateHealthState(int numDays) {
        if (water < requirements.minWater) {
            healthState = Health.Underwatered;
            ageHealthy = Math.Min(0, ageHealthy - numDays);
            updateHealthyColor(numDays, -1);
        } else if (requirements.minWater <= water && water <= requirements.maxWater) {
            ageHealthy += numDays;
            if (ageHealthy >= 0) {
                healthState = Health.Healthy;
            }
            updateHealthyColor(numDays, 1);
        } else {
            healthState = Health.Overwatered;
            ageHealthy = Math.Min(0, ageHealthy - numDays);
            updateHealthyColor(numDays, -1);
        }
        
    }

    // Update growth state based on number of days in a healthy state according to growth schedule
    // If plant grows, reset healthy age to 0 for next stage
    // Note: Plant can only grow if the color is fully recovered to white
    private void updateGrowthState() {
        Growth newGrowthState = growthState;
        if (spriteRenderer.color == Color.white) {
            switch (growthState) {
                case Growth.Sprout:
                    if (ageHealthy >= requirements.growthSchedule[0]) {
                        newGrowthState = Growth.Primary;
                        ageHealthy = 0;
                    }
                    break;
                case Growth.Primary:
                    if (ageHealthy >= requirements.growthSchedule[1]) {
                        newGrowthState = Growth.Secondary;
                        ageHealthy = 0;
                    }
                    break;
                case Growth.Secondary:
                    if (ageHealthy >= requirements.growthSchedule[2]) {
                        newGrowthState = Growth.Mature;
                        ageHealthy = 0;
                    }
                    break;
                case Growth.Mature:
                    if (requirements.growthSchedule.Length >= 4 && ageHealthy >= requirements.growthSchedule[3]) {
                        newGrowthState = Growth.Overgrown;
                        ageHealthy = 0;
                    }
                    break;
                default:
                    break;
            }
        }

        if (newGrowthState != growthState)
        {
            growthState = newGrowthState;
            EventBus.Broadcast<PlantStateManager>(EventTypes.AgedPlant, this);
        }
    }

    // After numDays days, water is absorbed based on the per-day evaporation raye
    private void absorb(int numDays)
    {
        water = Math.Max(water - (absorptionRate * numDays), 0);
    }

    // Formats the plant stats into a string
    private string formatStats()
    {
        return "Plant Name: "          + plantType.ToString()       + "\n"
             + "Age: "                 + age.ToString()             + "\n"
             + "Healthy Age: "         + ageHealthy.ToString()      + "\n"
             + "Health State: "        + healthState.ToString()     + "\n"
             + "Growth State: "        + growthState.ToString()     + "\n"
             + "Water: "               + water.ToString()           + "\n"
             + "Absorption Rate: "     + absorptionRate.ToString()  + "\n"
             + "Soil: "                + soil.ToString()            + "\n"
             + "Sun: "                 + sun.ToString()             + "\n";
    }

    private void displayStats()
    {
        stats.text = formatStats();
        displayWaterLevels();
    }

    public void displayWaterLevels() {
        waterMeter.GetComponent<RectTransform>().sizeDelta = new Vector2 (0, water / MAX_WATER_AMT);
    }

    private string getSpriteName()
    {
        string spriteName = plantType.ToString().ToLower() + "_";
        string lowerHealthState = healthState.ToString().ToLower();
        switch (growthState)
        {
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

    private Sprite getSprite(string spriteName)
    {
        foreach (Sprite plantSprite in plantSprites)
        {
            if (plantSprite.name == spriteName)
            {
                return plantSprite;
            }
        }
        Debug.Log("Could not find sprite " + spriteName);
        return null;
    }

    private void updateSprite()
    {
        string spriteName = getSpriteName();
        spriteRenderer.sprite = getSprite(spriteName);
    }
}
