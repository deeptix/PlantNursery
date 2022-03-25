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
    // [SerializeField] public TMP_Text trackNameText;
    // [SerializeField] public TMP_Text buggyFactText;
    // [SerializeField] public TMP_Text trackFactText;
    // [SerializeField] public GameObject trackSelectButton;
    // [SerializeField] public GameObject playerBuggy;
    // [SerializeField] public TMP_Text paginationText;

    // [SerializeField] public StatsUI speedStats;
    // [SerializeField] public StatsUI accelerationStats;

    private int selectionIndex;

    void Start() {
        // Loading track info from a JSON file
        TextAsset plantFile = Resources.Load<TextAsset>(plantsFilePath);
        plants = JsonUtility.FromJson<Plants>(plantFile.ToString()).plants;

        selectionIndex = 0;
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
        plantImage.sprite = currentPlant.PlantImage;
        plantNameText.text = currentPlant.name;
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

    // // Swaps back to buggy selection scene
    // public void back() {
    //     // fade in menu1 music, fade out menu2 music 
    //     StartCoroutine(sm.FadeAudio(sm.musicSource, 0.5f, sm.menu2Source.volume));
    //     StartCoroutine(sm.FadeAudio(sm.menu2Source, 1.0f, 0));
    //     sm.PlayUISound(SoundManager.UISound.BackButton); // ui sound
    //     currentSelection = SelectionItem.BUGGY;
    //     selectionIndex = 0;
    //     setCurrentScene();
    // }

    // // Swaps back to start screen
    // public void backToStart() {
    //     sm.PlayUISound(SoundManager.UISound.BackButton); // ui sound
    //     StartCoroutine(sm.SceneMusicFade(sm.musicSource, 0.5f)); // fade music between scenes 
    //     SceneManager.LoadScene("StartScreen");
    // }

    // // Selects the currently displayed item
    // public void select() {
    //     // play select button sound 
    //     sm.PlayUISound(SoundManager.UISound.SelectButton);

    //     switch (currentSelection) {
    //         case SelectionItem.BUGGY:
    //             // fade in menu2 music, fade out menu1 music 
    //             StartCoroutine(sm.FadeAudio(sm.menu2Source, 0.5f, sm.GetBGMVolume()));
    //             StartCoroutine(sm.FadeAudio(sm.musicSource, 1.0f, 0));
                
    //             PlayerSelections.SelectedBuggy = buggies[selectionIndex];
    //             currentSelection = SelectionItem.TRACK;
    //             selectionIndex = 0;
    //             setCurrentScene();
    //             break;
    //         case SelectionItem.TRACK:
    //             sm.musicSource.volume = sm.menu2Source.volume;
    //             sm.menu2Source.volume = 0f; 
    //             PlayerSelections.SelectedTrack = tracks[selectionIndex];
    //             RandomizeCPUBuggies();
    //             SceneManager.LoadScene("MainScene");
    //             break;
    //         default: Debug.Log("Error: Invalid selection"); break;
    //     }
    // }
}
