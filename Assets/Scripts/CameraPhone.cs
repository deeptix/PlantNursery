using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CameraPhone : MonoBehaviour
{
    public GameObject phone;
    public TMP_Text SearchText;
    public TMP_Text PlantNameText;
    public Button SearchButton;


    private bool isDragging;
    private Vector3 mouseOffsetPos;
    private float tolerance = 20;
    private Vector3 defaultPosition;
    private PhoneManager phoneManager;
    private EncyclopediaManager encyclopediaManager;
    private bool resetComplete = false;
    private PlantType plantType;

    public void OnMouseDown()
    {
        isDragging = true;
        Vector3 currentPos = new Vector3(transform.position.x, transform.position.y, 0);
        mouseOffsetPos = Camera.main.ScreenToWorldPoint(Input.mousePosition) - currentPos;
    }

    public void OnMouseUp()
    {
        isDragging = false;
    }

    void Start()
    {
        phoneManager = GameObject.Find("PhoneManager").GetComponent<PhoneManager>();
        encyclopediaManager = GameObject.Find("EncyclopediaManager").GetComponent<EncyclopediaManager>();

        defaultPosition = transform.position;
        SetDefaultUI();
    }

    public void GoToDefaultPosition()
    {
        transform.position = defaultPosition;
        SetDefaultUI();
        resetComplete = true;
    }

    void Update()
    {
        if (resetComplete) {
            Vector2 phoneOnScreenPos = Camera.main.WorldToScreenPoint(transform.position);
            if (phoneOnScreenPos.x < tolerance || phoneOnScreenPos.x > Screen.width - tolerance ||
                phoneOnScreenPos.y < tolerance || phoneOnScreenPos.y > Screen.height - tolerance)
            {
                phoneManager.HidePhoneCamera();
                phoneManager.ShowPhone();
                resetComplete = false;
            }
        }

        if (isDragging) {
            Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition) - (transform.position + mouseOffsetPos);
            transform.Translate(mousePosition);

            LayerMask mask = LayerMask.GetMask("Plants");
            RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.zero, Mathf.Infinity, mask);

            if (hit.collider != null)
            {
                SearchText.text = "Found!";
                plantType = hit.transform.gameObject.GetComponent<PlantStateManager>().plantType;
                PlantNameText.text = Enum.GetName(typeof(PlantType), plantType);
                SearchButton.interactable = true;
            }
            else
            {
                SetDefaultUI();
            }
        }
    }

    void SetDefaultUI() {
        SearchText.text = "Searching...";
        PlantNameText.text = "Unknown";
        SearchButton.interactable = false;
    }

    public void Search() {
        phoneManager.HidePhoneCamera();
        encyclopediaManager.ShowEncyclopedia();
        encyclopediaManager.GoToPage(plantType);

        resetComplete = false;
    }

    public void HidePhone() {
        phone.SetActive(false);
    }

    public void ShowPhone() {
        GoToDefaultPosition();
        phone.SetActive(true);
    }
}
