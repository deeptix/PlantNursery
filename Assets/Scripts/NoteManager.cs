using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class NoteManager : MonoBehaviour
{
    public float finalScale = 6.7f;
    public float transformTime = 1.5f;

    public GameObject noteText;
    public GameObject noteInput;
    public GameObject openButton;
    public GameObject closeButton;


    Vector3 oldPos;
    Vector3 oldScale;

    bool currentlyMoving = false;
    Vector3 newPos;
    Vector3 newScale;
    float currentTime = 0.0f;
    bool typing = false;

    void Start() {
        oldPos = transform.position;
        oldScale = transform.localScale;
        typing = false;
        SetNoteUI();
    }

    void Update() {
        if (currentlyMoving) {
            transform.position = Vector3.Lerp(transform.position, newPos, currentTime / transformTime);
            transform.localScale = Vector3.Lerp(transform.localScale, newScale, currentTime / transformTime);
            currentTime += Time.deltaTime;
            if (currentTime >= transformTime) {
                transform.position = newPos;
                transform.localScale = newScale;
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
        newPos = UnityEngine.Camera.main.transform.position;
        newPos.z = 0;
        newScale = new Vector3(finalScale, finalScale, finalScale);

        currentTime = 0.0f;
        currentlyMoving = true;

        typing = true;
        SetNoteUI();
        noteInput.GetComponent<TMP_InputField>().text = noteText.GetComponent<TMP_Text>().text;
        GetComponent<Canvas>().sortingOrder = 5;
    }

    public void ShrinkNote() {
        newPos = oldPos;
        newScale = oldScale;

        currentTime = 0.0f;
        currentlyMoving = true;

        typing = false;
        SetNoteUI();
        noteText.GetComponent<TMP_Text>().text = noteInput.GetComponent<TMP_InputField>().text;
        GetComponent<Canvas>().sortingOrder = 1;
    }
}
