using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public abstract class GameManager : MonoBehaviour
{
    public GameObject winConditions;
    public GameObject winScreen;

    private string plantsFilePath = "plants";
    private Plant[] plants;

    private Dictionary<PlantType, Plant> plantDict;

    protected PlantStateManager[] plantManagers;

    void Awake() {
        TextAsset plantFile = Resources.Load<TextAsset>(plantsFilePath);
        plants = JsonUtility.FromJson<Plants>(plantFile.ToString()).plants;

        plantDict = new Dictionary<PlantType, Plant>();
        foreach (Plant plant in plants) {
            plantDict.Add(plant.plantType, plant);

            if (plant.tasks.wateringTask != null)
                plant.tasks.wateringTask.SetPlantType(plant.plantType);

            if (plant.tasks.sunlightTask != null)
                plant.tasks.sunlightTask.SetPlantType(plant.plantType);
            
            if (plant.tasks.soilTask != null)
                plant.tasks.soilTask.SetPlantType(plant.plantType);
        }

        plantManagers = GameObject.FindObjectsOfType<PlantStateManager>();
        EventBus.AddListener<PlantStateManager>(EventTypes.UpdatedPlant, CheckWinCondition);

        winConditions.SetActive(true);
        winScreen.SetActive(false);
    }

    public void HideWinConditions() {
        winConditions.SetActive(false);
    }

    private void CheckWinCondition(PlantStateManager plant) {
        UpdateStats(plant);
        if (HasWon()) {
            winScreen.SetActive(true);
        }
    }

    public abstract void UpdateStats(PlantStateManager plant);
    public abstract bool HasWon();

    // Grabs the plant information from the dictionary given plant name
    // Returns true if successful and stores the result in plant; false otherwise
    public bool GetPlant(PlantType plantType, out Plant plant) {
        return plantDict.TryGetValue(plantType, out plant);
    }

    public Plant[] GetAllPlants() {
        return plants;
    }

    public void BackToLevelSelect() {
        SceneManager.LoadScene("LevelSelect");
    }
}
