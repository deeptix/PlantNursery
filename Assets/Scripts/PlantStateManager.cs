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
    private GameUIManager gameUIManager;
    private RectTransform rectTransform;
    private DrainageManager drainManager;
    private SpriteRenderer spriteRenderer;
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
    public GameObject polaroid;
    public TMP_Text polaroidDate;
    private float idealWater;         // minWater + maxWater / 2
    private Sprite lastSeenSprite; // last sprite seen before time skip
    private Color lastSeenColor; // last color of plant before time skip
    private Sprite currentSprite;
    private Color currentColor;

    public float SUNLIGHT_ADJ = 0.1f; // Absorption adjustment for incorrect sunlight
    public float SOIL_ADJ = 0.1f;     // Absorption adjustment for incorrect soil
    public float RED_ADJ = 0.2f;
    public float GREEN_ADJ = 0.1f;
    public float BLUE_ADJ = 0.1f;
    public float LOWEST_RED_COLOR = 0.3f;
    public float LOWEST_GREEN_COLOR = 0.1f;
    public float LOWEST_BLUE_COLOR = 0.1f;
    public float TOTAL_NEEDLE_ROT = 114; // Total Degree of Rotation (Z Axis)

    public GameObject waterMeterNeedle;

    private const float MAX_WATER_AMT = 10;

    void Awake()
    {
        plantSprites = Resources.LoadAll<Sprite>(plantType.ToString());
    }

    // Initialize plant
    void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        gameUIManager = GameObject.Find("GameUIManager").GetComponent<GameUIManager>();
        rectTransform = GetComponent<RectTransform>();
        drainManager = transform.Find("Pot").GetComponent<DrainageManager>();

        string childSpriteName = plantType.ToString() + "Plant";
        spriteRenderer = transform.Find(childSpriteName).GetComponent<SpriteRenderer>();

        // Store plant care requirements
        if (!gameManager.GetPlant(plantType, out requirements))
        {
            Debug.Log("Could not find plant.");
        }
        // Plant starts as a healthy sprout with no soil
        age = 0;
        healthState = Health.Healthy;
        growthState = Growth.Sprout;

        // TODO: correctly initialize soil
        // Currently initializes soil
        soil = requirements.Soil;

        // Plant starts off with the minimum acceptable amount of water
        //      --> forces watering on the first day
        idealWater = (requirements.minWater + requirements.maxWater) / 2;
        water = requirements.minWater;

        // just for debugging purposes --> updating here is unnecessary (only needs to be done in passTime)
        updateAbsorptionRate();

        // Display initial stats
        displayStats();

        // Adding listeners
        EventBus.AddListener(EventTypes.DayPassed, new CallBack<int>(passTime));
        EventBus.AddListener(EventTypes.ZoomedIn, new CallBack(RemoveLastSeen));

        lastSeenSprite = spriteRenderer.sprite;
        lastSeenColor = spriteRenderer.color;
        currentSprite = spriteRenderer.sprite;
        currentColor = spriteRenderer.color;
    }

    // Updates Growth and Health states based on plant care + number of days skipped
    public void passTime(int numDays)
    {
        // update last seens before passing time
        lastSeenSprite = spriteRenderer.sprite;
        lastSeenColor = spriteRenderer.color;
        polaroidDate.text = numDays + " day(s) ago";

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

        // Save current sprites and color after passing time
        currentSprite = spriteRenderer.sprite;
        currentColor = spriteRenderer.color;
    }

    /* Functions to update plant states based on user actions */

    // Updates amount of water plant holds when user waters the plant
    // Note: A plant can have a maximum of 10 water
    // Note: addWaterAmount can be the time duration in which the user watered the plant for
    public void waterPlant(float addWaterAmount) {
        water = Math.Min(water + addWaterAmount, MAX_WATER_AMT);
        if (water >= idealWater) {
            drainManager.turnDrainageOn();
        }
        displayStats();
    }

    public void movePlant(Sunlight newSun) {
        sun = newSun;

        // just for debugging purposes --> updating here is unnecessary (only needs to be done in passTime)
        updateAbsorptionRate();

        displayStats();
    }

    // Updates the soil when user changes the soil
    public void changeSoil(SoilTypes newSoil)
    {
        soil = newSoil;

        // just for debugging purposes --> updating here is unnecessary (only needs to be done in passTime)
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
                return Math.Min(Math.Max(LOWEST_RED_COLOR, newColorComp), 1);
            case "GREEN":
                return Math.Min(Math.Max(LOWEST_GREEN_COLOR, newColorComp), 1);
            case "BLUE":
                return Math.Min(Math.Max(LOWEST_BLUE_COLOR, newColorComp), 1);
            default:
                return Math.Min(Math.Max(0, newColorComp), 1);
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
        }
    }

    // Update health state based on water amount
    private void updateHealthState(int numDays)
    {
        if (water < requirements.minWater)
        {
            healthState = Health.Underwatered;
            ageHealthy = 0;
        }
        else if (requirements.minWater <= water && water <= requirements.maxWater)
        {
            // if plant was previously healthy and still healthy, update number of healthy days
            if (healthState == Health.Healthy)
            {
                ageHealthy += numDays;
            }
            healthState = Health.Healthy;
        }
        else
        {
            healthState = Health.Overwatered;
            ageHealthy = 0;
        }

        // if plant's external conditions are not correct, stop growth by setting ageHealthy to 0
        if (!correctExternalConditions())
        {
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
        waterMeterNeedle.GetComponent<RectTransform>().rotation = Quaternion.Euler(0,0, (((water / MAX_WATER_AMT) * TOTAL_NEEDLE_ROT) - TOTAL_NEEDLE_ROT/2) * -1);
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

    public void ShowPolaroid()
    {
        // check zoomed out and notes not open
        if (Camera.main.orthographicSize == 5 && !gameUIManager.GetNotesStatus())
        {
            polaroid.SetActive(true);
            spriteRenderer.sprite = lastSeenSprite;
            spriteRenderer.color = lastSeenColor;
        }
    }

    public void HidePolaroid() 
    {
        polaroid.SetActive(false);
        spriteRenderer.sprite = currentSprite;
        spriteRenderer.color = currentColor;
    }

    private void RemoveLastSeen() 
    {
        polaroid.SetActive(false);
        spriteRenderer.sprite = currentSprite;
        spriteRenderer.color = currentColor;
    }
}
