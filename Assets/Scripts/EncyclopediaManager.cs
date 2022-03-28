using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class EncyclopediaManager : MonoBehaviour
{
    public GameObject encyclopediaPanel;

    private string plantsFilePath = "plants";
    private Plant[] plants;

    public Image plantImage;
    public TMP_Text plantNameText;
    public TMP_Text environmentText;
    public TMP_Text soilText;
    public TMP_Text sunlightText;
    public TMP_Text wateringText;
    public TMP_Text repottingText;
    public TMP_Text potsizeText;

    private int selectionIndex;
    private bool showingEncyclopedia;

    void Start() {
        // Loading track info from a JSON file
        TextAsset plantFile = Resources.Load<TextAsset>(plantsFilePath);
        plants = JsonUtility.FromJson<Plants>(plantFile.ToString()).plants;

        selectionIndex = 0;
        showingEncyclopedia = false;
        HideEncyclopedia();
    }

    public void ToggleEncyclopedia() {
        showingEncyclopedia = !showingEncyclopedia;
        if (showingEncyclopedia) {
            ShowEncyclopedia();
        } else {
            HideEncyclopedia();
        }
    }

    private void ShowEncyclopedia() {
        setCurrentScene();
        encyclopediaPanel.SetActive(true);
    }

    private void HideEncyclopedia() {
        encyclopediaPanel.SetActive(false);
    }

    private void setCurrentScene() {
        Plant currentPlant = plants[selectionIndex];
        plantImage.preserveAspect = true;
        plantImage.sprite = currentPlant.PlantImage;
        plantNameText.text = currentPlant.name;
        environmentText.text = "Typical Environment: " + currentPlant.description.environmentInfo;
        soilText.text = "Soil Info: " + currentPlant.description.soilInfo;
        sunlightText.text = "Sunlight Needs: " + currentPlant.description.sunlightInfo;
        wateringText.text = "Watering Needs: " + currentPlant.description.wateringInfo;
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
}
