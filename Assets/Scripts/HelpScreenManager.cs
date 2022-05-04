using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class HelpScreenManager : MonoBehaviour
{
    public GameObject helpPanel;

    public TMP_Text winConditions;
    public TMP_Text winReminder;
    public Slider BGMControl;
    public Slider SFXControl;
    public AudioSource BGMPlayer;
    public AudioSource SFXPlayer;

    // Start is called before the first frame update
    void Start()
    {
        HideInfo();

    }

    public void ShowInfo()
    {
        SetCurrentScene();
        helpPanel.SetActive(true);
    }

    public void HideInfo()
    {
        helpPanel.SetActive(false);
    }

    private void SetCurrentScene()
    {
        winReminder.text = winConditions.text;
    }

    public void SetBGMVolume()
    {
        BGMPlayer.volume = BGMControl.normalizedValue;
    }
    public void SetSFXVolume()
    {
        SFXPlayer.volume = SFXControl.normalizedValue;
    }
}
