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
    Scallion
}

// Various soil types plants require
public enum SoilTypes {
    Loamy,
    Sandy,
    Peaty,
    Clay,
    Chalky,
    Silty
}

// Various amounts of sunlight plants require
public enum Sunlight {
    Sun,        // Full sunlight
    Partial,    // Partial shade/sunlight
    Shade       // Full shade
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
public class Plant
{
    public string name;                     // Name of the plant
    public string imageName;                // Name of the image for the plant
    public PlantDescription description;    // All the English description for the plant

    public PlantType plantType {
        get {
            return (PlantType)Enum.Parse(typeof(PlantType), name);
        }
    }

    // Insert any other numbers required for the plant
    // public SoilTypes soil;
    // public Sunlight sun;
    public int wateringSchedule;            // water every wateringSchedule weeks
    public float minWater;                 // minimum amount of water that plant needs
    public float maxWater;                 // maximum amount of water that plant can have


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
}

[System.Serializable]
public class Plants
{
    public Plant[] plants;
}
