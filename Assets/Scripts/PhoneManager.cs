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

    void Start()
    {
        showingPhone = false;
        HidePhoneCamera();
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
