using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PhoneManager : MonoBehaviour
{
    public GameObject phone;
    public CameraPhone cameraPhone;
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
            // gameUIManager.ShowToolbar();
        } else {
            ShowPhone();
            // gameUIManager.HideToolbar();
        }
    }

    public void HidePhone() {
        phone.SetActive(false); //TODO: animation
    }

    public void ShowPhone() {
        phone.SetActive(true); //TODO: animation
    }

    public void DeactivateCameraButton()
    {
        phone.transform.GetChild(0).GetComponent<Button>().interactable = false;
    }

    public void ActivateCameraButton()
    {
        phone.transform.GetChild(0).GetComponent<Button>().interactable = true;
    }

    public void ShowPhoneCamera() {
        cameraPhone.ShowPhone();
        phoneButton.SetActive(false);
    }

    public void HidePhoneCamera() {
        cameraPhone.HidePhone();
        phoneButton.SetActive(true);
    }

    public void HidePhoneButton() {
        phoneButton.SetActive(false);
    }

    public void ShowPhoneButton() {
        phoneButton.SetActive(true);
    }
}
