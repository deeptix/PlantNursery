using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class InfoButtonManager : MonoBehaviour
{
    public GameObject infoPanel;

    private string climateFilePath = "climate";
    private Climate[] climates;

    private int selectionIndex;
    private bool showingInfo;

    public TMP_Text regionText;
    public TMP_Text temperatureText;
    public TMP_Text humidityText;

    // Start is called before the first frame update
    void Start()
    {
        TextAsset climateFile = Resources.Load<TextAsset>(climateFilePath);
        climates = JsonUtility.FromJson<Climates>(climateFile.ToString()).climates;

        selectionIndex = 0;
        showingInfo = false;
        HideInfo();
    }

    public void ShowInfo() {
        setCurrentScene();
        infoPanel.SetActive(true);
    }

    public void HideInfo() {
        infoPanel.SetActive(false);
    }

    private void setCurrentScene() {
        Climate currentClimate = climates[selectionIndex];
        regionText.text = "Region: " + currentClimate.regionInfo;
        temperatureText.text = "Temperature: " + currentClimate.temperatureInfo;
        humidityText.text = "Humidity: " + currentClimate.humidityInfo;
    }
}
