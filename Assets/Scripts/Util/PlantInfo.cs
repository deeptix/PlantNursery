/**
 * Classes that are used for reading from a JSON file to load plant information
 */

using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public enum PlantType {
    DonkeysTail,
    SpiderPlant,
    Scallion,
    PeaceLily
}

// Various soil types plants require
// Ordered by ability to retain water, highest to lowest
public enum SoilTypes {
    Clay = 0,
    Loamy = 1,
    Sandy = 2,
    Silty = 3,
}

// Various amounts of sunlight plants require
// Ordered from least to most sunlight
public enum Sunlight {
    Shade = 0,      // Full shade
    Partial = 1,    // Partial shade/sunlight
    Sun = 2,        // Full sunlight
}

[System.Serializable]
public class PlantDescription
{
    public string environmentInfo;  // English description of environemnt info
    public string soilInfo;         // English description of soil info
    public string sunlightInfo;     // English description of sunlight info
    public string wateringInfo;     // English description of watering info
    public string repottingInfo;    // English description of repotting info
    public string pottingInfo;      // English description of pot size info
}

[System.Serializable]
public class PlantTasks
{
    public TaskDescription soilTask;
    public TaskDescription wateringTask;
    public TaskDescription sunlightTask;
}

[System.Serializable]
public class Plant
{
    public string name;                     // Name of the plant
    public string imageName;                // Name of the image for the plant
    public PlantDescription description;    // All the English description for the plant
    public PlantTasks tasks;                // All the Tasks associated for the plant

    public PlantType plantType {
        get {
            return (PlantType)Enum.Parse(typeof(PlantType), imageName);
        }
    }

    // Insert any other numbers required for the plant
    public string soil;                  // Soil required for plant

    public SoilTypes Soil {
        get {
            return (SoilTypes)Enum.Parse(typeof(SoilTypes), soil);
        }
    }

    public string sun;                    // Sunlight required for plant

    public Sunlight Sun {
        get {
            return (Sunlight)Enum.Parse(typeof(Sunlight), sun);
        }
    }

    // public int minTemp;                  // Minimum acceptable temperature for plant
    // public int maxTemp;                  // Maximum acceptable temperature for plant
    public int wateringSchedule;            // Water every wateringSchedule weeks
    public float minWater;                  // Minimum amount of water that plant needs
    public float maxWater;                  // Maximum amount of water that plant can have
    public int[] growthSchedule;            // Number of days spent in healthy state before growing


    private const string resourceFilePath = "Images";

    // Constructs the file path to the plant's image
    private string imageFilePath() {
        return resourceFilePath + "/" + imageName;
    }

    private Sprite plantImageSprite;

    public Sprite PlantImage {
        get {
            if (!plantImageSprite) // Load sprite
                plantImageSprite = Resources.Load<Sprite>(imageFilePath());
            return plantImageSprite;
        }
    }

    private Sprite[] plantSprites;

    public Sprite[] PlantSprites {
        get {
            if (plantSprites == null)
                plantSprites = Resources.LoadAll<Sprite>(plantType.ToString());
            return plantSprites;
        }
    }

    private string getSpriteName(Growth growthState, Health healthState) {
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

    public Sprite GetSprite(Growth growthState, Health healthState) {
        string spriteName = getSpriteName(growthState, healthState);
        foreach (Sprite plantSprite in PlantSprites) {
            if (plantSprite.name == spriteName) {
                return plantSprite;
            }
        }
        return null;
    }
}

[System.Serializable]
public class Plants
{
    public Plant[] plants;
}
