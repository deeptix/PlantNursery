using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToolsManager : MonoBehaviour
{
    public GameObject soil;
    public GameObject fertilizer;
    public GameObject wateringCan;
    public GameObject mist;

    private SoundManager soundManager;

    void Start()
    {
        soundManager = GameObject.Find("SoundManager").GetComponent<SoundManager>();
        DisableAllTools();        
    }

    public void DisableAllTools() 
    {
        soil.SetActive(false);    
        fertilizer.SetActive(false);
        wateringCan.SetActive(false);
        mist.SetActive(false);
    }

    public void ToggleSoil(bool active) 
    {
        DisableAllTools();
        soil.SetActive(active);
    }

    public void ToggleFertilizer(bool active) 
    {
        DisableAllTools();
        fertilizer.SetActive(active);
        if (active) soundManager.PlayFertilizerSelectSound();
    }

    public void ToggleWateringCan(bool active) 
    {
        DisableAllTools();
        wateringCan.SetActive(active);
        if (active) soundManager.PlayWaterSelectSound();
    }

    public void ToggleMist(bool active) 
    {
        DisableAllTools();
        mist.SetActive(active);
        if (active) soundManager.PlayMistSelectSound();
    }
}
