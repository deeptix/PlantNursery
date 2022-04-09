using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private string plantsFilePath = "plants";
    private Plant[] plants;

    private Dictionary<PlantType, Plant> plantDict;

    void Awake() {
        TextAsset plantFile = Resources.Load<TextAsset>(plantsFilePath);
        plants = JsonUtility.FromJson<Plants>(plantFile.ToString()).plants;

        plantDict = new Dictionary<PlantType, Plant>();
        foreach (Plant plant in plants) {
            plantDict.Add(plant.plantType, plant);
        }
    }

    // Grabs the plant information from the dictionary given plant name
    // Returns true if successful and stores the result in plant; false otherwise
    public bool GetPlant(PlantType plantType, out Plant plant) {
        return plantDict.TryGetValue(plantType, out plant);
    }

    public Plant[] GetAllPlants() {
        return plants;
    }
}