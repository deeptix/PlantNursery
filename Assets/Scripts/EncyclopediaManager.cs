using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class EncyclopediaManager : MonoBehaviour
{
    public GameObject encyclopediaPanel;

    public Image plantImage;
    public TMP_Text plantNameText;
    public TMP_Text environmentText;
    public TMP_Text soilText;
    public TMP_Text sunlightText;
    public TMP_Text wateringText;
    public TMP_Text repottingText;
    public TMP_Text potsizeText;

    private Plant[] plants;
    private int selectionIndex;
    private bool showingEncyclopedia;
    private PhoneManager phoneManager;
    private GameManager gameManager;

    private Dictionary<PlantType, int> plantIndices;

    void Start() {
        // Loading track info from a JSON file
        phoneManager = GameObject.Find("PhoneManager").GetComponent<PhoneManager>();
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        plants = gameManager.GetAllPlants();

        plantIndices = new Dictionary<PlantType, int>();
        int i = 0;
        foreach (Plant plant in plants) {
            plantIndices.Add(plant.plantType, i);
            i++;
        }

        selectionIndex = 0;
        showingEncyclopedia = false;
        HideEncyclopedia();
    }

    public void ShowEncyclopedia() {
        setCurrentScene();
        encyclopediaPanel.SetActive(true);
    }

    public void HideEncyclopedia() {
        encyclopediaPanel.SetActive(false);
    }

    private void setCurrentScene() {
        Plant currentPlant = plants[selectionIndex];
        PlantTasks tasks = currentPlant.tasks;
        plantImage.preserveAspect = true;
        plantImage.sprite = currentPlant.PlantImage;
        plantNameText.text = currentPlant.name;
        environmentText.text = "Typical Environment: " + currentPlant.description.environmentInfo;
        soilText.text = "Soil Info: " + currentPlant.description.soilInfo;
        sunlightText.text = "Sunlight Needs: " + currentPlant.description.sunlightInfo;

        if (tasks.wateringTask != null && !tasks.wateringTask.task.IsCompleted()) {
            wateringText.text = tasks.wateringTask.task.GetTaskName();
        } else {
            wateringText.text = "Watering Needs: " + currentPlant.description.wateringInfo;
        }

        repottingText.text = "Repotting Needs: " + currentPlant.description.repottingInfo;
        potsizeText.text = "Typical Pot Size: " + currentPlant.description.pottingInfo;
    }

    // Swaps to the next item in the list
    public void nextPlant() {
        selectionIndex = (selectionIndex + 1) % plants.Length;
        setCurrentScene();
    }

    // Swaps to the previous item in the list
    public void prevPlant() {
        selectionIndex = (selectionIndex + plants.Length - 1) % plants.Length;
        setCurrentScene();
    }

    public void GoToPage(PlantType plantType) {
        int index;
        if (!plantIndices.TryGetValue(plantType, out index)) {
            return;
        }

        selectionIndex = index;
        setCurrentScene();
    }
}
