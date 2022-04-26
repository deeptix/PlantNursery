using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StartManager : MonoBehaviour
{
    public GameObject creditsPanel;

    void Start() {
        HideCredits();
    }

    public void StartGame() {
        SceneManager.LoadScene("LevelSelect");
    }

    public void ShowCredits() {
        creditsPanel.SetActive(true);
    }

    public void HideCredits() {
        creditsPanel.SetActive(false);
    }
}
