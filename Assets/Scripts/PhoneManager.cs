using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhoneManager : MonoBehaviour
{
    public GameObject phone;
    public GameObject phoneCamera;
    public GameObject phoneButton;

    private bool showingPhone;
    private GameUIManager gameUIManager;
    
    void Start()
    {
        gameUIManager = GameObject.Find("GameUIManager").GetComponent<GameUIManager>();

        showingPhone = false;
        HidePhoneCamera();
        HidePhone();
    }

    public void TogglePhone() {
        showingPhone = !showingPhone;
        if (!showingPhone) {
            HidePhone();
            gameUIManager.ShowToolbar();
        } else {
            ShowPhone();
            gameUIManager.HideToolbar();
        }
    }

    public void HidePhone() {
        phone.SetActive(false); //TODO: animation
    }

    public void ShowPhone() {
        phone.SetActive(true); //TODO: animation
    }

    public void ShowPhoneCamera() {
        phoneCamera.SetActive(true); //TODO: animation
        phoneCamera.GetComponent<CameraPhone>().GoToDefaultPosition();
        phoneButton.SetActive(false);
    }

    public void HidePhoneCamera() {
        phoneCamera.SetActive(false); //TODO: animation
        phoneButton.SetActive(true);
    }
}
