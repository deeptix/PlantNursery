using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhoneManager : MonoBehaviour
{
    public GameObject phone;

    private bool showingPhone;
    private GameUIManager gameUIManager;
    
    void Start()
    {
        gameUIManager = GameObject.Find("GameUIManager").GetComponent<GameUIManager>();

        showingPhone = false;
        HidePhone();
    }

    public void TogglePhone() {
        showingPhone = !showingPhone;
        if (!showingPhone) {
            HidePhone();
        } else {
            ShowPhone();
        }
    }

    void HidePhone() {
        phone.SetActive(false); //TODO: animation
        gameUIManager.ShowToolbar();
    }

    void ShowPhone() {
        phone.SetActive(true); //TODO: animation
        gameUIManager.HideToolbar();
    }
}
