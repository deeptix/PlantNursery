using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class NoteManager : MonoBehaviour
{
    public float finalScale = 6.7f;
    public float transformTime = 0.2f;

    public GameObject noteText;
    public GameObject noteInput;
    public GameObject openButton;
    public GameObject closeButton;


    Vector3 oldPos;
    Vector3 oldScale;

    bool currentlyMoving = false;
    Vector3 startPos;
    Vector3 newPos;
    Vector3 startScale;
    Vector3 newScale;
    int newSortingOrder;
    float currentTime = 0.0f;
    bool typing = false;

    private Canvas canvas;
    private GameUIManager gameUIManager;

    void Start() {
        canvas = GetComponent<Canvas>();
        gameUIManager = GameObject.Find("GameUIManager").GetComponent<GameUIManager>();

        oldPos = transform.position;
        oldScale = transform.localScale;
        typing = false;
        SetNoteUI();
    }

    void Update() {
        if (currentlyMoving) {
            transform.position = Vector3.Slerp(startPos, newPos, currentTime / transformTime);
            transform.localScale = Vector3.Slerp(startScale, newScale, currentTime / transformTime);
            currentTime += Time.deltaTime;
            if (currentTime >= transformTime) {
                transform.position = newPos;
                transform.localScale = newScale;
                canvas.sortingOrder = newSortingOrder;
                currentlyMoving = false;
            }
        }
    }

    void SetNoteUI() {
        noteText.SetActive(!typing);
        noteInput.SetActive(typing);
        openButton.SetActive(!typing);
        closeButton.SetActive(typing);
    }

    public void ExpandNote() {
        oldPos = transform.position;
        oldScale = transform.localScale;

        startPos = transform.position;
        startScale = transform.localScale;

        newPos = UnityEngine.Camera.main.transform.position;
        newPos.z = 0;
        newScale = new Vector3(finalScale, finalScale, finalScale);
        newSortingOrder = 5;

        currentTime = 0.0f;
        currentlyMoving = true;

        typing = true;
        SetNoteUI();
        noteInput.GetComponent<TMP_InputField>().text = noteText.GetComponent<TMP_Text>().text;
        canvas.sortingOrder = newSortingOrder;

        gameUIManager.HideToolbar();
    }

    public void ShrinkNote() {
        startPos = transform.position;
        startScale = transform.localScale;

        newPos = oldPos;
        newScale = oldScale;
        newSortingOrder = 1;

        currentTime = 0.0f;
        currentlyMoving = true;

        typing = false;
        SetNoteUI();
        noteText.GetComponent<TMP_Text>().text = noteInput.GetComponent<TMP_InputField>().text;

        gameUIManager.ShowToolbar();
    }
}
