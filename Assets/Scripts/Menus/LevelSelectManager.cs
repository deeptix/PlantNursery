using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class LevelSelectManager : MonoBehaviour
{
    public TMP_Text levelText;
    public string[] levelNames;
    
    private int currentLevel;
    
    void Start()
    {
        currentLevel = 0;
        SetCurrentScene();
    }

    private void SetCurrentScene() {
        levelText.text = levelNames[currentLevel];
    }

    public void PrevLevel() {
        currentLevel = (currentLevel + levelNames.Length - 1) % levelNames.Length;
        SetCurrentScene();
    }

    public void NextLevel() {
        currentLevel = (currentLevel + 1) % levelNames.Length;
        SetCurrentScene();
    }

    public void SelectLevel() {
        SceneManager.LoadScene(levelNames[currentLevel]);
    }
}
