using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class EncyclopediaManager : MonoBehaviour
{
    public GameObject encyclopediaPanel;

    public TMP_Text plantNameText;
    public Image plantImage;

    public TMP_Text[] taskTexts;
    public Image[] taskCheckboxes;
    public Sprite completedCheckbox;
    public Sprite incompleteCheckbox;

    public Image[] soilIcons;
    public Image[] waterIcons;
    public GameObject waterIndicator;
    public Image[] sunlightIcons;

    public GameObject[] plantStates;

    private Plant[] plants;
    private int selectionIndex;
    private bool showingEncyclopedia;
    private PhoneManager phoneManager;
    private GameManager gameManager;

    private Dictionary<PlantType, int> plantIndices;
    private Dictionary<PlantType, Growth> maxPlantGrowth;

    private const float MAX_WATER_AMT = 10;
    private float indicatorMaxWidth;

    void Start() {
        // Loading track info from a JSON file
        phoneManager = GameObject.Find("PhoneManager").GetComponent<PhoneManager>();
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        plants = gameManager.GetAllPlants();
        indicatorMaxWidth = waterIndicator.GetComponent<RectTransform>().sizeDelta.x;

        plantIndices = new Dictionary<PlantType, int>();
        maxPlantGrowth = new Dictionary<PlantType, Growth>();

        int i = 0;
        foreach (Plant plant in plants) {
            plantIndices.Add(plant.plantType, i);
            maxPlantGrowth.Add(plant.plantType, Growth.Sprout);
            i++;
        }

        selectionIndex = 0;
        showingEncyclopedia = false;
        HideEncyclopedia();

        EventBus.AddListener<PlantStateManager>(EventTypes.AgedPlant, UpdateMaxGrowth);
    }

    public void UpdateMaxGrowth(PlantStateManager plant)
    {
        Growth plantGrowth;
        if (maxPlantGrowth.TryGetValue(plant.plantType, out plantGrowth)) {
            if ((int)plant.growthState > (int)plantGrowth) {
                maxPlantGrowth[plant.plantType] = plant.growthState;
            }
        }
    }

    public void ShowEncyclopedia() {
        setCurrentScene();
        encyclopediaPanel.SetActive(true);
    }

    public void HideEncyclopedia() {
        encyclopediaPanel.SetActive(false);
    }

    private void setTaskUI(TaskDescription taskDescription, int index) {
        if (taskDescription == null || taskDescription.task == null) {
            taskCheckboxes[index].enabled = false;
            taskTexts[index].enabled = false;
            return;
        }

        taskCheckboxes[index].enabled = true;
        taskTexts[index].enabled = true;

        if (taskDescription.task.IsCompleted()) {
            taskCheckboxes[index].sprite = completedCheckbox;
            taskTexts[index].color = new Color(0.4f, 0.4f, 0.4f, 1.0f);
            taskTexts[index].text = "<s>" + taskDescription.task.GetTaskName() + ": " + taskDescription.task.GetTaskProgressString() + "</s>";
        } else {
            taskCheckboxes[index].sprite = incompleteCheckbox;
            taskTexts[index].color = new Color(0.0f, 0.0f, 0.0f, 1.0f);
            taskTexts[index].text = taskDescription.task.GetTaskName() + ": " + taskDescription.task.GetTaskProgressString();
        }
    }

    private void setCurrentScene() {
        Plant currentPlant = plants[selectionIndex];
        PlantTasks tasks = currentPlant.tasks;

        plantNameText.text = currentPlant.name;
        plantImage.preserveAspect = true;
        plantImage.sprite = currentPlant.PlantImage;

        setTaskUI(tasks.soilTask, 0);
        setTaskUI(tasks.wateringTask, 1);
        setTaskUI(tasks.sunlightTask, 2);

        foreach (Image soilIcon in soilIcons) {
            soilIcon.color = new Color(0.3f, 0.3f, 0.3f, 0.3f);
        }

        if (tasks.soilTask.task != null && tasks.soilTask.task.IsCompleted()) {
            soilIcons[(int)currentPlant.Soil].color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
        }

        waterIndicator.GetComponent<RectTransform>().sizeDelta = new Vector2(0f, 0f);

        if (tasks.wateringTask.task != null && tasks.wateringTask.task.IsCompleted()) {
            float minRatio = currentPlant.minWater / MAX_WATER_AMT;
            float maxRatio = currentPlant.maxWater / MAX_WATER_AMT;
            float avgRatio = (maxRatio + minRatio) / 2.0f;
            Vector3 currentPos = waterIndicator.transform.localPosition;

            waterIndicator.GetComponent<RectTransform>().sizeDelta = new Vector2((maxRatio - minRatio) * indicatorMaxWidth, 0f);
            waterIndicator.transform.localPosition = new Vector3((minRatio * indicatorMaxWidth) - indicatorMaxWidth / 2, currentPos.y, currentPos.z);
        }

        foreach (Image sunlightIcon in sunlightIcons) {
            sunlightIcon.color = new Color(0.5f, 0.25f, 0.25f, 0.3f);
        }

        if (tasks.sunlightTask.task != null && tasks.sunlightTask.task.IsCompleted()) {
            sunlightIcons[(int)currentPlant.Sun].color = new Color(1.0f, 0.5f, 0.5f, 1.0f);
        }

        Growth plantGrowth;
        maxPlantGrowth.TryGetValue(currentPlant.plantType, out plantGrowth);

        int i = 0;
        for (; i <= Mathf.Min((int)plantGrowth, plantStates.Length - 1); i++) {
            Sprite plantSprite = currentPlant.GetSprite((Growth)i, Health.Healthy);
            if (plantSprite != null) {
                plantStates[i].SetActive(true);
                plantStates[i].GetComponent<Image>().sprite = plantSprite;
            } else {
                plantStates[i].SetActive(false);
            }
        }
        for (; i < plantStates.Length; i++) {
            plantStates[i].SetActive(false);
        }
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
